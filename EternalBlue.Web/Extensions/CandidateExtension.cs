using EternalBlue.Web.Models;
using EternalBlue.Web.ViewModels;

namespace EternalBlue.Web.Extensions
{
    public static class CandidateExtension
    {
        public static CandidateViewModel ToCandidateViewModel(this Candidate candidate, IList<Technology> technologies)
        {
            return new CandidateViewModel
            {
                CandidateId = candidate.CandidateId,
                FullName = candidate.FirstName + " " + candidate.LastName,
                Email = candidate.Email,
                ProfilePicture = candidate.ProfilePicture,
                Experience = candidate.Experience.Select(exp => {
                    var experience = exp.ToExperienceViewModel(technologies);
                    return experience.Description;
                })
            };
        }
    }
}
