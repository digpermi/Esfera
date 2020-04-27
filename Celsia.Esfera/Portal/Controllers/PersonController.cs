using Bussines;
using Bussines.Bussines;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Cache;
using Utilities.Messages;

namespace Portal.Controllers
{
    public class PersonController:Controller
    {
        private readonly IPersonBussines personBussines;
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly IIdentificationTypeBussines identificationTypeBussines;
        private readonly IInterestBussines interestBussines;
        private readonly IRelationshipBussines relationshipBussines;
        private readonly ICacheUtility cache;
        private readonly ILogger<PersonController> logger;

        public PersonViewModel TestPerson { get; set; }

        public PersonController(EsferaContext context, ILogger<PersonController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.personBussines = new PersonBussines(context);
            this.externalSystemBussines = new ExternalSystemBussines(context);
            this.identificationTypeBussines = new IdentificationTypeBussines(context);
            this.interestBussines = new InterestBussines(context);
            this.relationshipBussines = new RelationshipBussines(context);
        }

        // GET: Person
        public IActionResult Index()
        {
            ICollection<Person> persons = this.personBussines.GetAllPersons();

            var personsList = new List<PersonViewModel>();

            foreach (var person in persons)
            {
                var personItem = new PersonViewModel()
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Identification = person.Identification,
                    IdentificationType = person.IdentificationType.Name,
                    Interest = person.Interest.Name,
                    PhoneNumber = person.PhoneNumber,
                    Email = person.Email
                };
                personsList.Add(personItem);
            }

            return this.View(personsList);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();      

            PersonViewModel person = new PersonViewModel()
            {
                ExternalSystems = externalSystems,
                SystemId = 0,
                IdentificationTypes = identificationTypes,
                IdentificationTypeId = 0,
                Interests = interests,
                InterestId = 0
            };

            return this.View(person);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel person)
        {
            try
            {
                // TODO: Add insert logic here

                Person personAdd = new Person() {
                    Identification = person.Identification,
                    IdentificationTypeId = person.IdentificationTypeId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    PhoneNumber = person.PhoneNumber,
                    MobileNumber = person.MobileNumber,
                    Email = person.Email,
                    Birthdate = person.Birthdate,
                    InterestId = person.InterestId,
                    PolicyData = person.PolicyData,
                    ExternalSystemId = person.SystemId
                };

                var result = this.personBussines.AddAsync(personAdd);

                return RedirectToAction(nameof(Index));
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

            PersonViewModel personEdit = new PersonViewModel()
            {
                Identification = person.Identification,
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.PhoneNumber,
                MobileNumber = person.MobileNumber,
                Email = person.Email,
                Birthdate = person.Birthdate,
                InterestId = person.InterestId,
                PolicyData = person.PolicyData,
                ExternalSystems = externalSystems,
                SystemId = person.ExternalSystemId,
                IdentificationTypes = identificationTypes,
                IdentificationTypeId = person.IdentificationTypeId,
                Interests = interests
            };

            return View(personEdit);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonViewModel person)
        {
            try
            {
                // TODO: Add update logic here

                Person personEdit = new Person()
                {
                    Id = id,
                    Identification = person.Identification,
                    IdentificationTypeId = person.IdentificationTypeId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    PhoneNumber = person.PhoneNumber,
                    MobileNumber = person.MobileNumber,
                    Email = person.Email,
                    Birthdate = person.Birthdate,
                    InterestId = person.InterestId,
                    PolicyData = person.PolicyData,
                    ExternalSystemId = person.SystemId
                };

                var result = this.personBussines.EditAsync(personEdit);

                return RedirectToAction(nameof(Index));
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