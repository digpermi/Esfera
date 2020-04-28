using System;
using System.Diagnostics;
using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<HomeController> logger;


        public CustomerViewModel TestCustomer { get; set; }

        public HomeController(EsferaContext context, ILogger<HomeController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;

            this.customerBussines = new CustomerBussines(context);
        }

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
