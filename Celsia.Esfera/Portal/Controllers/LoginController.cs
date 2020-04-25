using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using Utilities.Cache;

namespace Portal.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> logger;
        private ICacheUtility cache;

        public LoginController(ILogger<LoginController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
