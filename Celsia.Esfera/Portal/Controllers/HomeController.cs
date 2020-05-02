using System.Diagnostics;
using Bussines;
using Bussines.Bussines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Configuration;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<HomeController> logger;
        private readonly ServiceConfig apiConfig;

        public CustomerViewModel TestCustomer { get; set; }

        public HomeController(EsferaContext context, ILogger<HomeController> logger, ICacheUtility cache, IOptions<ServiceConfig> apiConfigOption)
        {
            this.logger = logger;
            this.cache = cache;
            this.apiConfig = apiConfigOption.Value;

            this.customerBussines = new CustomerBussines(context);
        }

        // [Authorize(Roles = "Administrador")]
        [Authorize]
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
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
