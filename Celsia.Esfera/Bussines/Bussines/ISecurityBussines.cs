using Entities.Models;

namespace Bussines.Bussines
{
    public interface ISecurityBussines
    {
        ApplicationUser Authenticate(string userName, string password);
    }
}
