namespace EternalBlue.Web.ViewModels
{
    public class CandidateViewModel
    {
        public Guid CandidateId { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string ProfilePicture { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public IEnumerable<string> Experience { get; set; } = new List<string>();
    }
}
