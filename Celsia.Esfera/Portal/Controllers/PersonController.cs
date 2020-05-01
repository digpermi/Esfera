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
            ICollection<Person> persons = this.personBussines.GetAllPersonsNoVinculed();

            var personsList = new List<PersonViewModel>();

            foreach (var person in persons)
            {
                var personItem = new PersonViewModel()
                {
                    Person = person
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

            Person personInitial = new Person();
            personInitial.ExternalSystemId = 0;
            personInitial.IdentificationTypeId = 0;
            personInitial.InterestId = 0;


            PersonViewModel person = new PersonViewModel()
            {
                ExternalSystems = externalSystems,
                IdentificationTypes = identificationTypes,
                Interests = interests,
                Person = personInitial
            };

            return this.View(person);
        }

        // POST: Person/Create
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

                    personCreate.ExternalSystems = externalSystems;
                    personCreate.IdentificationTypes = identificationTypes;
                    personCreate.Interests = interests;

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

            PersonViewModel personEdit = new PersonViewModel()
            {
                Person = person,
                ExternalSystems = externalSystems,
                IdentificationTypes = identificationTypes,
                Interests = interests
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

                    personUpdate.ExternalSystems = externalSystems;
                    personUpdate.IdentificationTypes = identificationTypes;
                    personUpdate.Interests = interests;

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