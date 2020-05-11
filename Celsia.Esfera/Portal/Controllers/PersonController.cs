using System;
using System.Security.Claims;
using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            PersonNoVinculatedViewModel personNoVinculatedViewModel = new PersonNoVinculatedViewModel();

            try
            {
                if (this.TempData["personMessageCode"] != null)
                {
                    MessageCode messageCode = (MessageCode)this.TempData["personMessageCode"];
                    personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, messageCode);
                }

                personNoVinculatedViewModel.Persons = this.personBussines.GetAllPersonsNoVinculed();
            }
            catch (Exception exec)
            {
                personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personNoVinculatedViewModel.UserMesage.Text);
            }

            return this.View(personNoVinculatedViewModel);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            PersonNoVinculatedViewModel personNoVinculatedViewModel = new PersonNoVinculatedViewModel();

            try
            {
                personNoVinculatedViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personNoVinculatedViewModel.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personNoVinculatedViewModel.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();
                personNoVinculatedViewModel.Relationships = this.masterBussinesManager.RelationshipBussines.GetAllRelationships();
            }
            catch (Exception exec)
            {
                personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personNoVinculatedViewModel.UserMesage.Text);
            }

            return this.View(personNoVinculatedViewModel);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonNoVinculatedViewModel personNoVinculatedViewModel)
        {
            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                personNoVinculatedViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personNoVinculatedViewModel.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personNoVinculatedViewModel.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();

                if (this.ModelState.IsValid)
                {
                    Person person = this.personBussines.GetPersonByIdentification(personNoVinculatedViewModel.CurrentPerson.Identification);

                    if (person == null)
                    {
                        this.personBussines.Add(personNoVinculatedViewModel.CurrentPerson, userName);
                        this.TempData["personMessageCode"] = MessageCode.PersonAdded.GetHashCode();
                        return this.RedirectToAction("Index", "Person");
                    }
                    else
                    {
                        personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personNoVinculatedViewModel.CurrentPerson.Identification);
                    }
                }
            }
            catch (Exception exec)
            {
                personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personNoVinculatedViewModel.UserMesage.Text);
            }

            return this.View(personNoVinculatedViewModel);
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            PersonNoVinculatedViewModel personNoVinculatedViewModel = new PersonNoVinculatedViewModel();

            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                personNoVinculatedViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personNoVinculatedViewModel.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personNoVinculatedViewModel.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();

                personNoVinculatedViewModel.CurrentPerson = this.personBussines.GetPersonById(id);
            }
            catch (Exception exec)
            {
                personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personNoVinculatedViewModel.UserMesage.Text);
            }

            return this.View(personNoVinculatedViewModel);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonNoVinculatedViewModel personNoVinculatedViewModel)
        {
            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                personNoVinculatedViewModel.ExternalSystems = this.masterBussinesManager.ExternalSystemBussines.GetAllExternalSystems();
                personNoVinculatedViewModel.IdentificationTypes = this.masterBussinesManager.IdentificationTypeBussines.GetAllIdentificationTypes();
                personNoVinculatedViewModel.Interests = this.masterBussinesManager.InterestBussines.GetAllInterests();

                if (this.ModelState.IsValid)
                {
                    Person personValid = this.personBussines.GetPersonByIdentificationById(personNoVinculatedViewModel.CurrentPerson.Identification, personNoVinculatedViewModel.CurrentPerson.Id);

                    if (personValid != null)
                    {
                        personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.PersonExist, personNoVinculatedViewModel.CurrentPerson.Identification);
                    }
                    else
                    {
                        this.personBussines.Edit(personNoVinculatedViewModel.CurrentPerson, userName);
                        this.TempData["personMessageCode"] = MessageCode.PersonEdited.GetHashCode();
                        return this.RedirectToAction("Index", "Person");
                    }
                }
            }
            catch (Exception exec)
            {
                personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personNoVinculatedViewModel.UserMesage.Text);
            }

            return this.View(personNoVinculatedViewModel);
        }


        // POST: Person/Delete/5
        public ActionResult Delete(int id)
        {
            PersonNoVinculatedViewModel personNoVinculatedViewModel = new PersonNoVinculatedViewModel();

            try
            {
                string userName = this.User.FindFirst(ClaimTypes.Name).Value;

                this.personBussines.Delete(id, userName);
                this.TempData["personMessageCode"] = MessageCode.PersonDeleted.GetHashCode();
                return this.RedirectToAction("Index", "Person");
            }
            catch (Exception exec)
            {
                personNoVinculatedViewModel.UserMesage = new ApplicationMessage(this.cache, MessageCode.GeneralError);
                this.logger.LogError(exec, personNoVinculatedViewModel.UserMesage.Text);
            }

            return this.View(personNoVinculatedViewModel);
        }

    }
}