﻿using Bussines;
using Bussines.Bussines;
using Entities.Data;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using System;
using System.Threading.Tasks;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class CustomerController:Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<CustomerController> logger;

        public CustomerViewModel TestCustomer { get; set; }

        public CustomerController(EsferaContext context, ILogger<CustomerController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.customerBussines = new CustomerBussines(context);
        }

        // GET: Customer
        public IActionResult Index()
        {
           return this.View();
        }


        // GET: Customer/Details/5
        [HttpGet("{id}")]
        public ActionResult Details(int id)
        { 
            Customer customer = this.customerBussines.GetAllCustomersById(3);

            if (customer == null)
            {
                this.NotFound();
            }

            var resul = new CustomerViewModel()
            {
                Code = customer.Code,
                FistName = customer.FistName,
                LastName = customer.LastName,
                Identification = customer.Identification,
                IdentificationType = customer.IdentificationTypeNavigation.Name,
                Address = customer.Address,
                CellPhone = customer.CellPhone,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                PolicyData = customer.PolicyData,
                System = customer.SystemNavigation.Name
            };

            


            return View(this.TestCustomer);
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