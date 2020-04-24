using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Models;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private ICacheUtility cache;

        public CustomerViewModel TestCustomer { get; set; }

        public HomeController(ILogger<HomeController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;

            this.TestCustomer = new CustomerViewModel();
        }

        public IActionResult Index()
        {
            ApplicationMessage customerMessage;

            try
            {
                //ir a base de datos
                this.TestCustomer.SelectedCustomer = new Entities.Customer { Name = "Diego" };

                customerMessage = new ApplicationMessage(this.cache, MessageCode.CustomerqueryOk);
            }
            catch (Exception ex)
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.Test);
                this.logger.LogError(ex, customerMessage.Text);
            }

            this.TestCustomer.UserMesage = customerMessage;

            return this.View(this.TestCustomer);
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
