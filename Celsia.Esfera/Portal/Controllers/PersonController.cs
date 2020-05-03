using System;
using System.Collections.Generic;
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
    public class PersonController : Controller
    {
        private readonly IPersonBussines personBussines;
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly IIdentificationTypeBussines identificationTypeBussines;
        private readonly IInterestBussines interestBussines;

        private readonly ICacheUtility cache;
        private readonly ILogger<PersonController> logger;


        public PersonController(EsferaContext context, ILogger<PersonController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.personBussines = new PersonBussines(context, this.cache);
            this.externalSystemBussines = new ExternalSystemBussines(context);
            this.identificationTypeBussines = new IdentificationTypeBussines(context);
            this.interestBussines = new InterestBussines(context);
        }

        // GET: Person
        public IActionResult Index()
        {
            ApplicationMessage personMessage = new ApplicationMessage();
            List<PersonViewModel> viewModelList = new List<PersonViewModel>();

            try
            {
                if (this.TempData["Message"] is string s)
                {
                    personMessage = JsonConvert.DeserializeObject<ApplicationMessage>(s);
                }

                ICollection<Person> persons = this.personBussines.GetAllPersonsNoVinculed();

                foreach (Person person in persons)
                {
                    PersonViewModel personItem = new PersonViewModel()
                    {
                        Person = person,
                        UserMesage = personMessage
                    };
                    viewModelList.Add(personItem);
                }
            }
            catch
            {
                personMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                PersonViewModel personItem = new PersonViewModel()
                {
                    UserMesage = personMessage
                };
                viewModelList.Add(personItem);
            }

            return this.View(viewModelList);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            ApplicationMessage personMessage = new ApplicationMessage();
            PersonViewModel viewModel = new PersonViewModel();

            try
            {
                ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
                ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
                ICollection<Interest> interests = this.interestBussines.GetAllInterests();

                Person personInitial = new Person()
                {
                    ExternalSystemId = 0,
                    IdentificationTypeId = 0,
                    InterestId = 0,
                    Birthdate = null
                };

                viewModel.ExternalSystems = externalSystems;
                viewModel.IdentificationTypes = identificationTypes;
                viewModel.Interests = interests;
                viewModel.Person = personInitial;

            }
            catch
            {
                personMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                viewModel.UserMesage = personMessage;
            }

            return this.View(viewModel);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel personCreate)
        {
            ApplicationMessage personMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();

            personCreate.ExternalSystems = externalSystems;
            personCreate.IdentificationTypes = identificationTypes;
            personCreate.Interests = interests;

            try
            {
                // TODO: Add insert logic here

                if (this.ModelState.IsValid)
                {

                    Person person = this.personBussines.GetPersonByIdentification(personCreate.Person.Identification);

                    if (person == null)
                    {
                        Person result = this.personBussines.Add(personCreate.Person);
                        personMessage = new ApplicationMessage(this.cache, MessageCode.PersonAdded);
                        this.TempData["Message"] = JsonConvert.SerializeObject(personMessage);
                        return this.RedirectToAction("Index", "Person");
                    }
                    else
                    {
                        personMessage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personCreate.Person.Identification);
                        personCreate.UserMesage = personMessage;
                        return this.View(personCreate);
                    }
                }
                else
                {
                    return this.View(personCreate);
                }

            }
            catch
            {
                personMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                personCreate.UserMesage = personMessage;
                return this.View(personCreate);
            }
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            ApplicationMessage personMessage = new ApplicationMessage();
            PersonViewModel personEdit = new PersonViewModel();

            try
            {
                Person person = this.personBussines.GetPersonById(id);

                ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
                ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
                ICollection<Interest> interests = this.interestBussines.GetAllInterests();

                personEdit.Person = person;
                personEdit.ExternalSystems = externalSystems;
                personEdit.IdentificationTypes = identificationTypes;
                personEdit.Interests = interests;
            }
            catch
            {
                personMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                personEdit.UserMesage = personMessage;
            }

            return this.View(personEdit);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonViewModel personUpdate)
        {
            ApplicationMessage personMessage = new ApplicationMessage();
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            personUpdate.ExternalSystems = externalSystems;
            personUpdate.IdentificationTypes = identificationTypes;
            personUpdate.Interests = interests;

            try
            {
                // TODO: Add update logic here
                if (this.ModelState.IsValid)
                {
                    Person personValid = this.personBussines.GetPersonByIdentificationById(personUpdate.Person.Identification, id);

                    if (personValid != null)
                    {
                        personMessage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personUpdate.Person.Identification);
                        personUpdate.UserMesage = personMessage;
                        return this.View(personUpdate);
                    }
                    else
                    {
                        personUpdate.Person.Id = id;
                        Person result = this.personBussines.Edit(personUpdate.Person);
                        personMessage = new ApplicationMessage(this.cache, MessageCode.PersonEdited);
                        this.TempData["Message"] = JsonConvert.SerializeObject(personMessage);
                        return this.RedirectToAction("Index", "Person");
                    }
                }
                else
                {
                    return this.View(personUpdate);
                }

            }
            catch
            {
                personMessage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                personUpdate.UserMesage = personMessage;
                return this.View(personUpdate);
            }
        }


        // POST: Person/Delete/5
        public ActionResult Delete(int id)
        {
            ApplicationMessage personMessage = new ApplicationMessage();

            try
            {
                // TODO: Add delete logic here

                Person result = this.personBussines.Delete(id);
                personMessage = new ApplicationMessage(this.cache, MessageCode.PersonDeleted);
                this.TempData["Message"] = JsonConvert.SerializeObject(personMessage);
                return this.RedirectToAction("Index", "Person");
            }
            catch
            {
                return this.View();
            }
        }

    }
}