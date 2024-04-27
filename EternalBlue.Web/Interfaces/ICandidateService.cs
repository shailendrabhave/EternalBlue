using EternalBlue.Web.Models;

namespace EternalBlue.Web.Interfaces
{
    public interface ICandidateService
    {
        Task<Candidate> GetAvailableCandidateAsync(IEnumerable<Experience> requiredExperience);
        void UpdateCandidateStatus(Guid candidateId, bool isSelected);
        Task<IEnumerable<Candidate>> GetSelectedCandidateAsync();
    }
}
