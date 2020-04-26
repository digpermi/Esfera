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

        // GET: Customer/Create
        public ActionResult Create()
        {
            ICollection<ExternalSystem> externalSystems = this.externalSystemBussines.GetAllExternalSystems();
            ICollection<IdentificationType> identificationTypes = this.identificationTypeBussines.GetAllIdentificationTypes();
            ICollection<Interest> interests = this.interestBussines.GetAllInterests();
            ICollection<Relationship> relationships = this.relationshipBussines.GetAllRelationships();
            

            PersonViewModel person = new PersonViewModel()
            {
                ExternalSystems = externalSystems,
                SystemId = 0,
                IdentificationTypes = identificationTypes,
                IdentificationTypeId = 0,
                Interests = interests,
                InterestId = 0,
                Relationships = relationships,
                RelationshipId = 0
            };

            return this.View(person);
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

    }
}