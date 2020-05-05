using System;
using System.Collections.Generic;
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
    public class PersonController : Controller
    {
        private readonly IPersonBussines personBussines;
        private readonly IMasterBussinesManager masterBussinesManager;
        private readonly ICacheUtility cache;
        private readonly ILogger<PersonController> logger;


        public PersonController(EsferaContext context, ILogger<PersonController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.personBussines = new PersonBussines(context, this.cache);
            this.masterBussinesManager = new MasterBussinesManager(context);
        }

        // GET: Person
        public IActionResult Index()
        {
            List<PersonViewModel> viewModelList = new List<PersonViewModel>();

            try
            {
                ApplicationMessage personMessage = new ApplicationMessage();

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
            catch (Exception exec)
            {
                PersonViewModel personItem = new PersonViewModel()
                {
                    UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError)
                };
                viewModelList.Add(personItem);
                this.logger.LogError(exec, personItem.UserMesage.Text);
            }

            return this.View(viewModelList);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            PersonViewModel viewModel = new PersonViewModel();
            try
            {
                viewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                viewModel.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                viewModel.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
                viewModel.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();

                viewModel.Person = new Person
                {
                    ExternalSystemId = 0,
                    IdentificationTypeId = 0,
                    InterestId = 0,
                    Birthdate = null
                };
            }
            catch (Exception exec)
            {
                viewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, viewModel.UserMesage.Text);
            }

            return this.View(viewModel);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel personCreate)
        {
            string userName = this.User.FindFirst(ClaimTypes.Name).Value;

            personCreate.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
            personCreate.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
            personCreate.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();

            try
            {
                // TODO: Add insert logic here
                if (this.ModelState.IsValid)
                {
                    Person person = this.personBussines.GetPersonByIdentification(personCreate.Person.Identification);

                    if (person == null)
                    {
                        Person result = this.personBussines.Add(personCreate.Person,userName);
                        this.TempData["Message"] = JsonConvert.SerializeObject(new ApplicationMessage(this.cache, MessageCode.PersonAdded));
                        return this.RedirectToAction("Index", "Person");
                    }
                    else
                    {
                        personCreate.UserMesage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personCreate.Person.Identification);
                    }
                }
            }
            catch (Exception exec)
            {
                personCreate.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personCreate.UserMesage.Text);
            }

            return this.View(personCreate);
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            PersonViewModel personEdit = new PersonViewModel();

            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                personEdit.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personEdit.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personEdit.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();

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

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonViewModel personEdit)
        {
            string userName = this.User.FindFirst(ClaimTypes.Name).Value;

            personEdit.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
            personEdit.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
            personEdit.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
            personEdit.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();

            try
            {
                // TODO: Add update logic here
                if (this.ModelState.IsValid)
                {
                    Person personValid = this.personBussines.GetPersonByIdentificationById(personEdit.Person.Identification, personEdit.Person.Id);

                    if (personValid != null)
                    {
                        personEdit.UserMesage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personEdit.Person.Identification);
                    }
                    else
                    {
                        this.personBussines.Edit(personEdit.Person, userName);
                        this.TempData["Message"] = JsonConvert.SerializeObject(new ApplicationMessage(this.cache, MessageCode.PersonEdited));
                        return this.RedirectToAction("Index", "Person");
                    }
                }
            }
            catch (Exception exec)
            {
                personEdit.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personEdit.UserMesage.Text);
            }

            return this.View(personEdit);
        }


        // POST: Person/Delete/5
        public ActionResult Delete(int id)
        {
            PersonViewModel personDelete = new PersonViewModel();

            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                this.personBussines.Delete(id, userName);
                this.TempData["Message"] = JsonConvert.SerializeObject(new ApplicationMessage(this.cache, MessageCode.PersonDeleted));
                return this.RedirectToAction("Index", "Person");
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