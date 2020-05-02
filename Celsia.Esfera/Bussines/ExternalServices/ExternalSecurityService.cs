using System.Threading.Tasks;
using Bussines.Adapters;
using Entities.Models;
using SecurityService;
using Utilities.Configuration;

namespace Bussines.ExternalServices
{
    internal class ExternalSecurityService : IExternalSecurityService
    {
        private readonly IApplicationUserAdapter adapter;
        private readonly ServiceConfig serviceConfig;

        public ExternalSecurityService(ServiceConfig serviceConfig)
        {
            this.adapter = new ApplicationUserAdapter();
            this.serviceConfig = serviceConfig;
        }


        public async Task<ApplicationUser> AuthenticateAsync(string userName, string password)
        {
            ApplicationUser applicationUser = null;

            AuthenticationClient service = new AuthenticationClient(AuthenticationClient.EndpointConfiguration.BasicHttpBinding_IAuthentication, this.serviceConfig.Address);

            try
            {
                loginSORequest loginRequest = new loginSORequest
                {
                    AppCode = this.serviceConfig.ApplicationCode,
                    password = password,
                    userName = userName
                };

                loginSOResponse loginResponse = await service.loginSOAsync(loginRequest);
                applicationUser = this.adapter.Adapt(loginResponse.loginSOResult);
            }
            finally
            {
                await service.CloseAsync();
            }

            return applicationUser;
        }


    }
}
