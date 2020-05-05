using System.Security.Claims;
using System.Threading.Tasks;
using Bussines.ExternalServices;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Utilities.Configuration;

namespace Bussines.Bussines
{
    public class SecurityBussines : Repository<ApplicationUser, EsferaContext>, ISecurityBussines
    {
        private readonly IExternalSecurityService externalSecurityService;

        public SecurityBussines(EsferaContext context, ServiceConfig serviceConfig) : base(context)
        {
            this.externalSecurityService = new ExternalSecurityService(serviceConfig);
        }

        public ApplicationUser Authenticate(string userName, string password)
        {
            Task<ApplicationUser> task = this.externalSecurityService.AuthenticateAsync(userName, password);
            task.Wait();
            return task.Result;
        }

        public ClaimsPrincipal GetUserPrincipalClaims(ApplicationUser applicationUser)
        {
            ClaimsIdentity identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, applicationUser.Name));
            identity.AddClaim(new Claim(ClaimTypes.Email, applicationUser.Email));
            identity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, applicationUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, applicationUser.Roles[0].ToString()));

            return new ClaimsPrincipal(identity);
        }
    }
}
