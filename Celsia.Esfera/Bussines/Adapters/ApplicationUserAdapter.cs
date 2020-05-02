using Entities.Models;
using SecurityService;

namespace Bussines.Adapters
{
    internal class ApplicationUserAdapter : IApplicationUserAdapter
    {
        public ApplicationUser Adapt(SecurityObject serviceEntity)
        {
            ApplicationUser applicationUser = null;
            if (!string.IsNullOrWhiteSpace(serviceEntity.SessionId))
            {
                applicationUser = new ApplicationUser
                {
                    UserName = serviceEntity.UserName,
                    Id = serviceEntity.SessionId,
                    Name = serviceEntity.Name,
                    Email = serviceEntity.Email,
                    Document = serviceEntity.DocumentNumber,
                    Roles = serviceEntity.Roles,
                };
            }

            return applicationUser;
        }
    }
}
