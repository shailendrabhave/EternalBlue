namespace EternalBlue.Web.Exceptions
{
    public class UnableToLoadTechnologiesException : Exception
    {
        public UnableToLoadTechnologiesException() : base("Unable to load technologies from EternalBlue API.")
        {
        }
    }
}
