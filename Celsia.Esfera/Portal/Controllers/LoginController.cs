using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
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
            try
            {
                if (this.ModelState.IsValid)
                {
                    ApplicationUser applicationUser = this.securityBussines.Authenticate(userViewModel.UsuarioLogin.UserName, userViewModel.Password);
                    if (applicationUser != null)
                    {
                        ClaimsPrincipal userPrincipal = this.securityBussines.GetUserPrincipalClaims(applicationUser);
                        await this.HttpContext.SignInAsync(userPrincipal);

                        return this.RedirectToAction(nameof(CustomerController.Index), "Customer");
                    }
                    else
                    {
                        userViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.InvalidLogin);
                    }
                }
            }
            catch (Exception exec)
            {
                userViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, userViewModel.UserMesage.Text);
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
