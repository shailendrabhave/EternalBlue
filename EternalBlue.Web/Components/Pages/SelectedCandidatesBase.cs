using BlazorBootstrap;
using EternalBlue.Web.Extensions;
using EternalBlue.Web.Interfaces;
using EternalBlue.Web.Models;
using EternalBlue.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace EternalBlue.Web.Components.Pages
{
    public class SelectedCandidatesBase : ComponentBase
    {
        [Inject]
        public required ICandidateService CandidateService { get; set; }
        [Inject]
        public required ITechnologyService TechnologyService { get; set; }
        public IList<Technology> Technologies { get; set; }
        public Grid<CandidateViewModel> grid = default!;
        private IEnumerable<CandidateViewModel> SelectedCandidates = default!;

        protected override async Task OnInitializedAsync()
        {
            Technologies = (await TechnologyService.GetTechnologiesAsync()).ToList();
            var candidates = await CandidateService.GetSelectedCandidateAsync();

            SelectedCandidates = candidates.Select(c => c.ToCandidateViewModel(Technologies)).ToList();
        }

        public async Task<GridDataProviderResult<CandidateViewModel>> SelectedCandidatesDataProvider(GridDataProviderRequest<CandidateViewModel> request)
        {
            return await Task.FromResult(request.ApplyTo(SelectedCandidates));
        }
    }
}
