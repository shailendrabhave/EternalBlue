namespace EternalBlue.Web.Exceptions
{
    public class UnableToLoadCandidatesException : Exception
    {
        public UnableToLoadCandidatesException() : base("Unable to load candidates from EternalBlue API.")
        {
        }
    }
}
