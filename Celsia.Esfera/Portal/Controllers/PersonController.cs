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
        private readonly ICacheUtility cache;
        private readonly ILogger<PersonController> logger;

        public PersonViewModel TestPerson { get; set; }

        public PersonController(EsferaContext context, ILogger<PersonController> logger, ICacheUtility cache)
        {
            this.logger = logger;
            this.cache = cache;
            this.personBussines = new PersonBussines(context);
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
                    Code = person.Code,
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

    }
}