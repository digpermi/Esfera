using System.Threading.Tasks;
using Bussines.ExternalServices;
using Entities.Models;
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
    }
}
