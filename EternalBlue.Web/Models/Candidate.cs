namespace EternalBlue.Web.Models
{
    public class Candidate
    {
        public Guid CandidateId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string ProfilePicture { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public IEnumerable<Experience> Experience { get; set; } = new List<Experience>();
        public CandidateSelectionStatus SelectionStatus { get; set; }
    }
}
