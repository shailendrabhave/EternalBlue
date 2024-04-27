namespace EternalBlue.Web.ViewModels
{
    public class ExperienceViewModel
    {
        public Guid TechnologyId { get; set; }
        public string TechnologyName { get; set; } = String.Empty;
        public int YearsOfExperience { get; set; }
        public string Description => $"{TechnologyName} ({YearsOfExperience.ToString()} { (YearsOfExperience > 1 ? "years" : "year")})";
    }
}
