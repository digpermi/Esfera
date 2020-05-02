using System;
using System.Collections.Generic;
using System.Security.Claims;
using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly IPersonBussines personBussines;
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly IIdentificationTypeBussines identificationTypeBussines;
        private readonly IInterestBussines interestBussines;
        private readonly IRelationshipBussines relationshipBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<CustomerController> logger;

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
            ApplicationMessage customerMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            var viewModel = new CustomerViewModel();
            viewModel.ExternalSystems = externalSystems;

            try
            {
                if (this.TempData["Message"] is string s)
                {
                    customerMessage = JsonConvert.DeserializeObject<ApplicationMessage>(s);
                }

                Customer customerInitial = new Customer();
                customerInitial.ExternalSystemId = 0;

                viewModel.UserMesage = customerMessage;
                viewModel.Customer = customerInitial;
            }
            catch
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                viewModel.UserMesage = customerMessage;
            }

            return this.View(viewModel);

        }

        // POST: Customer/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int code, byte externalsystemid)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;

            ApplicationMessage customerMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            var viewModel = new CustomerViewModel();
            viewModel.ExternalSystems = externalSystems;
            
            try
            {
                if (externalsystemid != 0 && code != 0)
                {
                    Customer customer = this.customerBussines.GetCustomer(code, externalsystemid,userName);

                    if (customer == null)
                    {
                        customerMessage = new ApplicationMessage(this.cache, MessageCode.CustomerNotFound);
                        viewModel.UserMesage = customerMessage;
                    }
                    else
                    {
                        ICollection<Person> persons = this.personBussines.GetAllPersonsVinculed(customer.Id);
                        viewModel.Customer = customer;
                        viewModel.Customer.Persons = persons;
                    }
                }
                else
                {
                    if (externalsystemid == 0)
                    {
                        ModelState.AddModelError("Customer.ExternalSystemId", "Campo requerido");
                    }
                    if (code == 0)
                    {
                        ModelState.AddModelError("Customer.Code", "Campo requerido");
                    }
                }
            }
            catch
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                viewModel.UserMesage = customerMessage;
            }

            return this.View(viewModel);

        }


        // GET: Customer/Create/5
        public ActionResult Create(int id)
        {
            ApplicationMessage customerMessage = new ApplicationMessage();
            PersonViewModel viewModel = new PersonViewModel();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();
            viewModel.ExternalSystems = externalSystems;
            viewModel.IdentificationTypes = identificationTypes;
            viewModel.Interests = interests;
            viewModel.Relationships = relationships;

            try
            {

                Customer customer = this.customerBussines.GetCustomerById(id);

                Person personInitial = new Person();
                personInitial.ExternalSystemId = customer.ExternalSystemId;
                personInitial.Code = customer.Code;
                personInitial.CustomerId = customer.Id;
                personInitial.IdentificationTypeId = 0;
                personInitial.InterestId = 0;
                personInitial.RelationshipId = 0;

                viewModel.Person = personInitial;

            }
            catch
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                viewModel.UserMesage = customerMessage;
            }

            return this.View(viewModel);
        }


        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel personCreate)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;

            ApplicationMessage customerMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();

            personCreate.ExternalSystems = externalSystems;
            personCreate.IdentificationTypes = identificationTypes;
            personCreate.Interests = interests;
            personCreate.Relationships = relationships;

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (personCreate.Person.RelationshipId == null)
                    {
                        ModelState.AddModelError("Person.RelationshipId", "Campo requerido");
                        return View(personCreate);
                    }
                    else
                    {
                        Person person = this.personBussines.GetPersonByIdentification(personCreate.Person.Identification);

                        if (person == null)
                        {
                            var result = this.personBussines.Add(personCreate.Person, userName);
                            customerMessage = new ApplicationMessage(this.cache, MessageCode.PersonAdded);
                            TempData["Message"] = JsonConvert.SerializeObject(customerMessage);
                            return RedirectToAction("Index", "Customer");
                        }
                        else
                        {
                            customerMessage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personCreate.Person.Identification);
                            personCreate.UserMesage = customerMessage;
                            return View(personCreate);
                        }
                    }
                }
                else
                {
                    return View(personCreate);
                }
            }
            catch
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                personCreate.UserMesage = customerMessage;
                return View(personCreate);
            }
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            ApplicationMessage customerMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();
            PersonViewModel personEdit = new PersonViewModel();
            personEdit.ExternalSystems = externalSystems;
            personEdit.IdentificationTypes = identificationTypes;
            personEdit.Interests = interests;
            personEdit.Relationships = relationships;

            try
            {
                Person person = this.personBussines.GetPersonById(id);

                personEdit.Person = person;
            }
            catch
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                personEdit.UserMesage = customerMessage;
            }

            return this.View(personEdit);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonViewModel personUpdate)
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            ApplicationMessage customerMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();
            personUpdate.ExternalSystems = externalSystems;
            personUpdate.IdentificationTypes = identificationTypes;
            personUpdate.Interests = interests;
            personUpdate.Relationships = relationships;

            try
            {
                if (this.ModelState.IsValid)
                {
                    if (personUpdate.Person.RelationshipId == null)
                    {
                        ModelState.AddModelError("Person.RelationshipId", "Campo requerido");
                        return View(personUpdate);
                    }
                    else
                    {
                        Person personValid = this.personBussines.GetPersonByIdentificationById(personUpdate.Person.Identification, id);

                        if (personValid != null)
                        {
                            customerMessage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personUpdate.Person.Identification);
                            personUpdate.UserMesage = customerMessage;
                            return View(personUpdate);
                        }
                        else
                        {
                            personUpdate.Person.Id = id;
                            var result = this.personBussines.Edit(personUpdate.Person,userName);
                            customerMessage = new ApplicationMessage(this.cache, MessageCode.PersonEdited);
                            TempData["Message"] = JsonConvert.SerializeObject(customerMessage);
                            return RedirectToAction("Index", "Customer");
                        }
                    }

                }
                else
                {
                    return View(personUpdate);
                }
            }
            catch
            {
                customerMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                personUpdate.UserMesage = customerMessage;
                return View(personUpdate);
            }
        }

        // POST: Person/Delete/5
        public ActionResult Delete(int id)
        {
            ApplicationMessage personMessage = new ApplicationMessage();
            var userName = User.FindFirst(ClaimTypes.Name).Value;

            try
            {
                // TODO: Add delete logic here

                var result = this.personBussines.Delete(id,userName);
                personMessage = new ApplicationMessage(this.cache, MessageCode.PersonDeleted);
                TempData["Message"] = JsonConvert.SerializeObject(personMessage);
                return RedirectToAction("Index", "Customer");
            }
            catch
            {
                return this.View();
            }
        }
    }
}