using System.Security.Claims;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface ISecurityBussines
    {
        ApplicationUser Authenticate(string userName, string password);

        ClaimsPrincipal GetUserPrincipalClaims(ApplicationUser applicationUser);
    }
}
