using System.Threading.Tasks;
using Entities.Models;

namespace Bussines.ExternalServices
{
    internal interface IExternalSecurityService
    {
        Task<ApplicationUser> AuthenticateAsync(string userName, string password);
    }
}