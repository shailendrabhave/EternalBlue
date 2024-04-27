namespace EternalBlue.Web.Exceptions
{
    public class CandidateNotFoundException : Exception
    {
        public CandidateNotFoundException() : base("Candidate not found.")
        {
        }
    }
}
