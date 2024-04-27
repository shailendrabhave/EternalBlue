using BlazorBootstrap;
using EternalBlue.Web.Extensions;
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

        public IList<string> ErrorMessages { get; set; }
        public CandidateViewModel Candidate { get; set; }
        public IList<Technology> Technologies { get; set; }
        public IList<ExperienceViewModel> SelectedExperienceList { get; set; }

        public int SelectedYearsOfExperience { get; set; }
        public Guid SelectedTechnologyId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ErrorMessages = new List<string>();
            SelectedExperienceList = new List<ExperienceViewModel>();
            SelectedYearsOfExperience = 1;
            Technologies = (await TechnologyService.GetTechnologiesAsync()).ToList();
            await GetNextAvailableCandidate();
        }

        private async Task GetNextAvailableCandidate()
        {
            var selectedExperience = SelectedExperienceList.Select(e => 
                new Experience
                { 
                    TechnologyId = e.TechnologyId, 
                    YearsOfExperience = e.YearsOfExperience 
                });
            var availableCandidate = (await CandidateService.GetAvailableCandidateAsync(selectedExperience));

            if (availableCandidate == null)
            {
                Candidate = null;
                return;
            }

            Candidate = availableCandidate.ToCandidateViewModel(Technologies);
        }

        public void SelectTechnology(ChangeEventArgs args)
        {
            SelectedTechnologyId = new Guid(args.Value.ToString());
        }

        public void SelectyearsOfExperience(ChangeEventArgs args) 
        {
            if(String.IsNullOrEmpty(args.Value.ToString()))
            {
                SelectedYearsOfExperience = 0;
            }
            else
            {
                SelectedYearsOfExperience = int.Parse(args.Value.ToString());
            }            
        }

        public async Task AddExperienceFilter()
        {
            var isValidExeperienceFilter = ValidateExperienceFilters();
            if (!isValidExeperienceFilter)
            {
                return;
            }
            
            var selectedExperience = new Experience { TechnologyId = SelectedTechnologyId, YearsOfExperience = SelectedYearsOfExperience };
            var selectedCandidateIndex = SelectedExperienceList.ToList().FindIndex(x => x.TechnologyId == SelectedTechnologyId);

            //Check if technology already added to filter. If yes replace the years of experience 
            if (selectedCandidateIndex < 0)
            {                
                SelectedExperienceList.Add(selectedExperience.ToExperienceViewModel(Technologies));
            }
            else
            {
                SelectedExperienceList[selectedCandidateIndex] = selectedExperience.ToExperienceViewModel(Technologies);
            }
            await GetNextAvailableCandidate();
        }

        private bool ValidateExperienceFilters()
        {
            ErrorMessages.Clear();
            bool isValid = true;

            if (SelectedTechnologyId == Guid.Empty)
            {
                isValid = false;
                ErrorMessages.Add("Please select a technology");
            }
                
            if (SelectedYearsOfExperience == 0)
            {
                isValid = false;
                ErrorMessages.Add("Please enter years of experience greater than 0");

            }

            return isValid;
        }

        public async Task RemoveExperience(Guid technologyId)
        {
            SelectedExperienceList = SelectedExperienceList.Where(e => e.TechnologyId != technologyId).ToList();
            await GetNextAvailableCandidate();
        }        

        public async Task ClearFilter()
        {
            ErrorMessages.Clear();
            SelectedExperienceList.Clear();
            SelectedYearsOfExperience = 0;
            SelectedTechnologyId = Guid.Empty;
            await GetNextAvailableCandidate();
        }
        public async Task SetCandidateStatus(bool isSelected)
        {
            CandidateService.UpdateCandidateStatus(Candidate.CandidateId, isSelected);
            await GetNextAvailableCandidate();
        }
    }
}
