using EternalBlue.Web.Interfaces;
using EternalBlue.Web.Models;
using EternalBlue.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace EternalBlue.Web.Components.Pages
{
    public class HomeBase : ComponentBase
    {
        [Inject]
        public required ICandidateService CandidateService { get; set; }
        [Inject]
        public required ITechnologyService TechnologyService { get; set; }

        public Candidate Candidate { get; set; }
        public IList<Technology> Technologies { get; set; }
        public IList<Experience> SelectedExperiences { get; set; }

        public async Task GetCandidate()
        {
            SelectedExperiences = new List<Experience>();
            Technologies = (await TechnologyService.GetTechnologiesAsync()).ToList();
            Candidate = (await CandidateService.GetAvailableCandidateAsync(SelectedExperiences));
        }

        public async Task FilterCandidate()
        {
            SelectedExperiences = new List<Experience>
            {
                new Experience { TechnologyId = new Guid("3B85BE83-9B4E-4B15-9EB2-68363936CA13"), YearsOfExperience = 1 },
                new Experience { TechnologyId = new Guid("3B85BE83-9B4E-4B15-9EB2-68363936CA18"), YearsOfExperience = 2 },
            }; 
            Candidate = (await CandidateService.GetAvailableCandidateAsync(SelectedExperiences));
        }

        public async Task UpdateCandidate()
        {
            CandidateService.UpdateCandidateStatus(Candidate.CandidateId, true);
            Candidate = (await CandidateService.GetAvailableCandidateAsync(SelectedExperiences));
        }
    }
}
