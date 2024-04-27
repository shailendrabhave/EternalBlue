namespace EternalBlue.Web.Exceptions
{
    public class TechnologyNotFoundException : Exception
    {
        public TechnologyNotFoundException() : base("Technology not found.")
        {
        }
    }
}
