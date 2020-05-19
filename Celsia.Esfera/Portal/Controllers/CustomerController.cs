using System;
using System.Security.Claims;
using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Portal.ViewModels;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    [Authorize(Roles = "Administrador,Usuario")]
    public class CustomerController : Controller
    {
        private readonly ICustomerBussines customerBussines;
        private readonly IPersonBussines personBussines;
        private readonly IMasterBussinesManager masterBussinesManager;
        private readonly ICacheUtility cache;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(EsferaContext context, ILogger<CustomerController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;

            this.customerBussines = new CustomerBussines(context);
            this.personBussines = new PersonBussines(context, this.cache);
            this.masterBussinesManager = new MasterBussinesManager(context);
        }

        public ActionResult Index()
        {
            CustomerViewModel CustomerViewModel = new CustomerViewModel();

            try
            {
                CustomerViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();

                if (this.TempData["currentCustomerId"] != null)
                {
                    int customeId = (int)this.TempData["currentCustomerId"];

                    CustomerViewModel.Customer = this.customerBussines.GetCustomerById(customeId);

                    if (CustomerViewModel.Customer != null)
                    {
                        CustomerViewModel.Code = CustomerViewModel.Customer.Code;
                        CustomerViewModel.ExternalSystemId = CustomerViewModel.Customer.ExternalSystemId;
                    }
                }

                if (this.TempData["personMessageCode"] != null)
                {
                    MessageCode messageCode = (MessageCode)this.TempData["personMessageCode"];
                    CustomerViewModel.UserMesage = new ApplicationMessage(this.cache, messageCode);
                }
            }
            catch (Exception exec)
            {
                CustomerViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, CustomerViewModel.UserMesage.Text);
            }

            return this.View(CustomerViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CustomerViewModel customerViewModel)
        {
            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                customerViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();

                if (this.ModelState.IsValid)
                {
                    Customer customer = this.customerBussines.GetCustomer(customerViewModel.Code.Value, customerViewModel.ExternalSystemId, userName);

                    if (customer != null)
                    {
                        customerViewModel.Customer = customer;
                    }
                    else
                    {
                        customerViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.CustomerNotFound);
                    }
                }
            }
            catch (Exception exec)
            {
                customerViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, customerViewModel.UserMesage.Text);
            }

            return this.View(customerViewModel);

        }

        public ActionResult Create(int id)
        {
            PersonViewModel personViewModel = new PersonViewModel();

            try
            {
                personViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personViewModel.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personViewModel.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
                personViewModel.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();

                Customer customer = this.customerBussines.GetCustomerById(id);

                personViewModel.Person = new Person
                {
                    ExternalSystemId = customer.ExternalSystemId,
                    Code = customer.Code,
                    CustomerId = customer.Id
                };
            }
            catch (Exception exec)
            {
                personViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personViewModel.UserMesage.Text);
            }

            return this.View(personViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel personCreate)
        {
            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                personCreate.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personCreate.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personCreate.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
                personCreate.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();

                if (this.ModelState.IsValid && personCreate.Person.RelationshipId != 0)
                {
                    Person person = this.personBussines.GetPersonByIdentification(personCreate.Person.Identification);

                    if (person == null)
                    {
                        this.personBussines.Add(personCreate.Person, userName);

                        this.TempData["currentCustomerId"] = personCreate.Person.CustomerId;
                        this.TempData["Message"] = JsonConvert.SerializeObject(new ApplicationMessage(this.cache, MessageCode.PersonAdded));

                        return this.RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        personCreate.UserMesage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personCreate.Person.Identification);
                    }
                }
                else if (personCreate.Person.RelationshipId == 0)
                {
                    this.ModelState.AddModelError("Person.RelationshipId", "Campo requerido");
                }

            }
            catch (Exception exec)
            {
                personCreate.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personCreate.UserMesage.Text);
            }

            return this.View(personCreate);
        }

        public ActionResult Edit(int id)
        {
            PersonViewModel personEdit = new PersonViewModel();

            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                personEdit.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personEdit.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personEdit.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
                personEdit.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();

                Person person = this.personBussines.GetPersonById(id);

                personEdit.Person = person;
            }
            catch (Exception exec)
            {
                personEdit.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personEdit.UserMesage.Text);
            }

            return this.View(personEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonViewModel personEdit)
        {
            try
            {
                personEdit.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personEdit.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personEdit.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
                personEdit.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();

                if (this.ModelState.IsValid && personEdit.Person.RelationshipId != 0)
                {
                    Person personValid = this.personBussines.GetPersonByIdentificationById(personEdit.Person.Identification, personEdit.Person.Id);

                    if (personValid == null)
                    {
                        int customerId = personEdit.Person.CustomerId.Value;

                        string userName = this.User.FindFirst(ClaimTypes.Name).Value;
                        this.personBussines.Edit(personEdit.Person, userName);

                        this.TempData["currentCustomerId"] = customerId;
                        this.TempData["personMessageCode"] = MessageCode.PersonEdited.GetHashCode();

                        return this.RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        personEdit.UserMesage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personEdit.Person.Identification);
                    }
                }
                else if (personEdit.Person.RelationshipId == 0)
                {
                    this.ModelState.AddModelError("Person.RelationshipId", "Campo requerido");
                }
            }
            catch (Exception exec)
            {
                personEdit.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personEdit.UserMesage.Text);
            }

            return this.View(personEdit);
        }

        public ActionResult Delete(int id)
        {
            PersonViewModel personDelete = new PersonViewModel();

            try
            {
                int customerId = this.customerBussines.GetCustomerIdByPersonId(id);

                string userName = this.User.FindFirst(ClaimTypes.Name).Value;
                this.personBussines.Delete(id, userName);

                this.TempData["currentCustomerId"] = customerId;
                this.TempData["personMessageCode"] = MessageCode.PersonDeleted.GetHashCode();

                return this.RedirectToAction("Index", "Customer");
            }
            catch (Exception exec)
            {
                personDelete.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personDelete.UserMesage.Text);
            }

            return this.View(personDelete);
        }
    }
}