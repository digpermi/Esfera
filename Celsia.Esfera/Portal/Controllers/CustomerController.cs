using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using System.Collections.Generic;
using Utilities.Cache;

namespace Portal.Controllers
{
    public class CustomerController:Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<CustomerController> logger;

        public CustomerViewModel TestCustomer { get; set; }

        public CustomerController(EsferaContext context, ILogger<CustomerController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.customerBussines = new CustomerBussines(context);
            this.externalSystemBussines = new ExternalSystemBussines(context);
        }

        // GET: Customer/Create
        public ActionResult Index()
        {
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();

            Customer customerInitial = new Customer();
            customerInitial.ExternalSystemId = 0;

            var resul = new CustomerViewModel()
            {
                ExternalSystems = externalSystems,
                Customer = customerInitial
            };
            return View(resul);
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]  // GET: Customer
        public IActionResult Index(CustomerViewModel customerView)
        {
            var resul = new CustomerViewModel();

            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();

            Customer customer = this.customerBussines.GetCustomer(customerView.Customer.Code, customerView.Customer.ExternalSystemId.Value);

            if (customer == null)
            {
                this.NotFound();
            }
            else
            {
                resul.Customer = customer;
                resul.ExternalSystems = externalSystems;
            }

            return this.View(resul);
        }



        // GET: Customer/Details/5
        [HttpGet("{id}")]
        public ActionResult Details()
        { 
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}