using EternalBlue.Web.Exceptions;
using EternalBlue.Web.Interfaces;
using EternalBlue.Web.Models;
using EternalBlue.Web.State;
using System.Text.Json;

namespace EternalBlue.Web.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TechnologyService> _logger;
        private readonly ApplicationState _applicationState;

        public TechnologyService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<TechnologyService> logger,
            ApplicationState applicationState)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _applicationState = applicationState;
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesAsync()
        {
            //Check if technologies data is already loaded into application state. Return if available.
            if (_applicationState.Technologies.Any())
            {
                return _applicationState.Technologies;
            }

            var requestUrl = _configuration.GetValue<string>("EternalBlueAPIEndpoints:Technologies");
            var httpResponseMessage = await _httpClient.GetAsync(requestUrl);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError("Error while reading technologies data from EternalBlue API: {ErrorMessage}", httpResponseMessage.ReasonPhrase);
                throw new UnableToLoadTechnologiesException();
            }

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            _applicationState.Technologies = await JsonSerializer.DeserializeAsync<IList<Technology>>(contentStream, options);
            if (_applicationState.Technologies == null)
            {
                _logger.LogError("Unable to desierialize technologies");
                throw new UnableToLoadTechnologiesException();
            }
            return _applicationState.Technologies;
        }
    }
}