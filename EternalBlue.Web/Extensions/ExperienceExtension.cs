using EternalBlue.Web.Exceptions;
using EternalBlue.Web.Models;
using EternalBlue.Web.ViewModels;

namespace EternalBlue.Web.Extensions
{
    public static class ExperienceExtension
    {
        public static ExperienceViewModel ToExperienceViewModel(this Experience experience, IList<Technology> technologies)
        {
            var technology = technologies.FirstOrDefault(t => t.Guid == experience.TechnologyId);
            if (technology == null)
            {
                throw new TechnologyNotFoundException();
            }

            return new ExperienceViewModel
            {
                TechnologyId = experience.TechnologyId,
                TechnologyName = technology.Name,
                YearsOfExperience = experience.YearsOfExperience
            };
        }
    }
}
