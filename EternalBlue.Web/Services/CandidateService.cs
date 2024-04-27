using EternalBlue.Web.Exceptions;
using EternalBlue.Web.Interfaces;
using EternalBlue.Web.Models;
using EternalBlue.Web.State;
using System.Text.Json;

namespace EternalBlue.Web.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CandidateService> _logger;
        private readonly ApplicationState _applicationState;

        public CandidateService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<CandidateService> logger,
            ApplicationState applicationState)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _applicationState = applicationState;
        }

        private async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
        {
            //Check if candidates data is already loaded into application state. Return if available.
            if (_applicationState.Candidates.Any())
            {
                return _applicationState.Candidates;
            }

            var requestUrl = _configuration.GetValue<string>("EternalBlueAPIEndpoints:Candidates");
            var httpResponseMessage = await _httpClient.GetAsync(requestUrl);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError("Error while reading candidates data from EternalBlue API: {ErrorMessage}", httpResponseMessage.ReasonPhrase);
                throw new UnableToLoadCandidatesException();
            }

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _applicationState.Candidates = await JsonSerializer.DeserializeAsync<IList<Candidate>>(contentStream, options);
            if (_applicationState.Candidates == null)
            {
                _logger.LogError("Unable to desierialize candidates");
                throw new UnableToLoadCandidatesException();
            }
            return _applicationState.Candidates;
        }

        public async Task<Candidate> GetAvailableCandidateAsync(IEnumerable<Experience> requiredExperience)
        {
            var allCandidates = await GetAllCandidatesAsync();

            //Return first available candidate if technologies filter is not applied
            if (!requiredExperience.Any())
            {
                return allCandidates.FirstOrDefault(c =>
                c.SelectionStatus == CandidateSelectionStatus.Pending);
            }

            return allCandidates.FirstOrDefault(c =>
                c.SelectionStatus == CandidateSelectionStatus.Pending &&
                requiredExperience.All(re =>
                    c.Experience.Any(e =>
                        e.TechnologyId == re.TechnologyId &&
                        e.YearsOfExperience >= re.YearsOfExperience))
                );
        }

        public void UpdateCandidateStatus(Guid candidateId, bool isSelected)
        {
            var candidateIndex = _applicationState.Candidates.ToList()
                .FindIndex(candidate => candidate.CandidateId == candidateId);

            if (candidateIndex < 0)
            {
                _logger.LogError("Invalid candidate id: {CandidateId}", candidateId.ToString());
                throw new CandidateNotFoundException();
            }

            _applicationState.Candidates[candidateIndex].SelectionStatus =
                isSelected ? CandidateSelectionStatus.Accepted : CandidateSelectionStatus.Rejected;
        }

        public async Task<IEnumerable<Candidate>> GetSelectedCandidateAsync()
        {
            var allCandidates = await GetAllCandidatesAsync();

            return allCandidates.Where(c => c.SelectionStatus == CandidateSelectionStatus.Accepted);
        }
    }
}