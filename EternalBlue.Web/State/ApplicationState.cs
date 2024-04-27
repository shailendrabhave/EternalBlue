using EternalBlue.Web.Models;

namespace EternalBlue.Web.State
{
    public class ApplicationState
    {
        public ApplicationState()
        {
            Candidates = new List<Candidate>();
            Technologies = new List<Technology>();
        }

        public IList<Candidate> Candidates { get; set; }
        public IList<Technology> Technologies { get; set; }
    }
}
