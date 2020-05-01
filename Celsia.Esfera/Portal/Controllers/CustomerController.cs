﻿using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using System.Collections.Generic;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class CustomerController:Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly IPersonBussines personBussines;
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly IIdentificationTypeBussines identificationTypeBussines;
        private readonly IInterestBussines interestBussines;
        private readonly IRelationshipBussines relationshipBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<CustomerController> logger;

        public CustomerViewModel TestCustomer { get; set; }

        public CustomerController(EsferaContext context, ILogger<CustomerController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.customerBussines = new CustomerBussines(context);
            this.personBussines = new PersonBussines(context, this.cache);
            this.externalSystemBussines = new ExternalSystemBussines(context);
            this.externalSystemBussines = new ExternalSystemBussines(context);
            this.identificationTypeBussines = new IdentificationTypeBussines(context);
            this.interestBussines = new InterestBussines(context);
            this.relationshipBussines = new RelationshipBussines(context);
        }

        // GET: Customer/Index
        public ActionResult Index()
        {
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();

            Customer customerInitial = new Customer();
            customerInitial.ExternalSystemId = 0;

            var viewModel = new CustomerViewModel()
            {
                ExternalSystems = externalSystems,
                Customer = customerInitial
            };
            return View(viewModel);
        }

        // POST: Customer/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int code, byte externalsystemid)
        {
            var viewModel = new CustomerViewModel();

            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            Customer customer = this.customerBussines.GetCustomer(code, externalsystemid);

            if (customer == null)
            {
                ApplicationMessage customerMessage = new ApplicationMessage(this.cache, MessageCode.CustomerNotFound);
                viewModel.UserMesage = customerMessage;
                viewModel.ExternalSystems = externalSystems;
            }
            else
            {
                ICollection<Person> persons = this.personBussines.GetAllPersonsVinculed(customer.Id);
                viewModel.Customer = customer;
                viewModel.ExternalSystems = externalSystems;
                viewModel.Customer.Persons = persons;
            }

            return this.View(viewModel);
        }


        // GET: Customer/Create/5
        public ActionResult Create(int id)
        {
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();
            Customer customer = this.customerBussines.GetCustomerById(id);

            Person personInitial = new Person();
            personInitial.ExternalSystemId = customer.ExternalSystemId;
            personInitial.Code = customer.Code;
            personInitial.CustomerId = customer.Id;
            personInitial.IdentificationTypeId = 0;
            personInitial.InterestId = 0;
            personInitial.RelationshipId = 0;


            PersonViewModel person = new PersonViewModel()
            {
                ExternalSystems = externalSystems,
                IdentificationTypes = identificationTypes,
                Interests = interests,
                Relationships = relationships,
                Person = personInitial
            };

            return this.View(person);
        }


        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel personCreate)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var result = this.personBussines.AddAsync(personCreate.Person);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
                    ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
                    ICollection<Interest> interests = this.interestBussines.GetAllInterests();
                    ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();

                    personCreate.ExternalSystems = externalSystems;
                    personCreate.IdentificationTypes = identificationTypes;
                    personCreate.Interests = interests;
                    personCreate.Relationships = relationships;

                    return View(personCreate);
                }

                
            }
            catch
            {
                return View();
            }
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            Person person = this.personBussines.GetPersonById(id);

            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();

            PersonViewModel personEdit = new PersonViewModel()
            {
                Person = person,
                ExternalSystems = externalSystems,
                IdentificationTypes = identificationTypes,
                Interests = interests,
                Relationships = relationships
            };

            return View(personEdit);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonViewModel personUpdate)
        {
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
                    personUpdate.Person.Id = id;
                    var result = this.personBussines.EditAsync(personUpdate.Person);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
                    ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
                    ICollection<Interest> interests = this.interestBussines.GetAllInterests();
                    ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();

                    personUpdate.ExternalSystems = externalSystems;
                    personUpdate.IdentificationTypes = identificationTypes;
                    personUpdate.Interests = interests;
                    personUpdate.Relationships = relationships;

                    return View(personUpdate);
                }
                
            }
            catch
            {
                return View();
            }
        }

        // POST: Person/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                var result = this.personBussines.DeleteAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}