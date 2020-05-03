using System.Security.Claims;
using System.Threading.Tasks;
using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Configuration;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> logger;
        private readonly ISecurityBussines securityBussines;
        private readonly ICacheUtility cache;

        public LoginController(EsferaContext context, ILogger<LoginController> logger, ICacheUtility cache, IOptions<ServiceConfig> serviceConfig)
        {
            this.logger = logger;
            this.cache = cache;
            this.securityBussines = new SecurityBussines(context, serviceConfig.Value);
        }

        public IActionResult Index()
        {
            UserLoginViewModel userViewModel = new UserLoginViewModel();
            return this.View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLoginViewModel userViewModel)
        {
            ApplicationMessage loginMessage = new ApplicationMessage();

            if (this.ModelState.IsValid)
            {
                ApplicationUser applicationUser = this.securityBussines.Authenticate(userViewModel.UsuarioLogin.UserName, userViewModel.Password);
                if (applicationUser != null)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, applicationUser.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Email, applicationUser.Email));
                    identity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, applicationUser.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.Role, applicationUser.Roles[0].ToString()));                   

                    ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                    await this.HttpContext.SignInAsync(userPrincipal);

                    return this.RedirectToAction(nameof(CustomerController.Index), "Customer");
                }
                else
                {
                    loginMessage = new ApplicationMessage(this.cache, MessageCode.InvalidLogin);
                    userViewModel.UserMesage = loginMessage;
                }
            }

            return this.View(userViewModel);

        }

        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();
            return this.RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
