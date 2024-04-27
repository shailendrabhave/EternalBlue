using EternalBlue.Web.Models;

namespace EternalBlue.Web.Interfaces
{
    public interface ITechnologyService
    {
        Task<IEnumerable<Technology>> GetTechnologiesAsync();
    }
}
