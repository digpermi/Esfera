using System.Collections.Generic;
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
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();

            Customer customerInitial = new Customer();
            customerInitial.ExternalSystemId = 0;

            CustomerViewModel viewModel = new CustomerViewModel()
            {
                ExternalSystems = externalSystems,
                Customer = customerInitial
            };
            return this.View(viewModel);
        }

        // POST: Customer/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int code, byte externalsystemid)
        {
            CustomerViewModel viewModel = new CustomerViewModel();

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
                if (this.ModelState.IsValid)
                {
                    Person result = this.personBussines.Add(personCreate.Person);

                    return this.RedirectToAction(nameof(Index));
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

                    return this.View(personCreate);
                }


            }
            catch
            {
                return this.View();
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

            return this.View(personEdit);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonViewModel personUpdate)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    personUpdate.Person.Id = id;
                    Person result = this.personBussines.Edit(personUpdate.Person);

                    return this.RedirectToAction(nameof(Index));
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

                    return this.View(personUpdate);
                }

            }
            catch
            {
                return this.View();
            }
        }

        // POST: Person/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Person result = this.personBussines.Delete(id);

                return this.RedirectToAction(nameof(Index));
            }
            catch
            {
                return this.View();
            }
        }
    }
}