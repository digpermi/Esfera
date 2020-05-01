using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using Utilities.Cache;
using Utilities.File;
using Utilities.Messages;

namespace Bussines.Bussines
{
    public class PersonBussines : IPersonBussines
    {
        private readonly IRepository<Person> repository;
        private readonly ICustomerBussines customerBussines;
        private readonly ICacheUtility cache;

        public PersonBussines(EsferaContext context, ICacheUtility cache)
        {
            this.repository = new PersonRepository(context);
            this.customerBussines = new CustomerBussines(context);
            this.cache = cache;
        }

        /// <summary>
        /// Busca todas las personas no vinculadas
        /// </summary>
        /// <returns></returns>
        public ICollection<Person> GetAllPersonsNoVinculed()
        {
            Task<List<Person>> task = this.repository.GetAsync(x => x.CustomerId == null, includeProperties: "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
            return task.Result;
        }

        /// <summary>
        /// Busca todas las personas no vinculadas
        /// </summary>
        /// <returns></returns>
        public ICollection<Person> GetAllPersonsVinculed(int customerId)
        {
            Task<List<Person>> task = this.repository.GetAsync(x => x.CustomerId == customerId, includeProperties: "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
            return task.Result;
        }

        /// <summary>
        /// Busca persona por id
        /// </summary>
        /// <returns></returns>
        public Person GetPersonById(int Id)
        {
            Task<List<Person>> task = this.repository.GetAsync(x => x.Id.Equals(Id));
            return task.Result.FirstOrDefault();
        }


        /// <summary>
        /// Inserta una persona
        /// </summary>
        /// <returns></returns>
        public Person AddAsync(Person person)
        {
            Task<Person> task = this.repository.AddAsync(person);
            return task.Result;
        }

        /// <summary>
        /// Editar una persona
        /// </summary>
        /// <returns></returns>
        public Person EditAsync(Person person)
        {
            Task<Person> task = this.repository.UpdateAsync(person);
            return task.Result;
        }

        /// <summary>
        /// Eliminar una persona
        /// </summary>
        /// <returns></returns>
        public Person DeleteAsync(int Id)
        {
            Task<Person> task = this.repository.DeleteAsync(Id);
            return task.Result;
        }

        public void UploadVinculatedPersons(string fileName, IValidator validator)
        {
            CsvFile<Person> csvFile = new CsvFile<Person>(new CsvPersonMapper());

            List<Person> persons = csvFile.ParseCSVFile(fileName).ToList();


            List<ApplicationMessage> processMessages = this.ProcessViculatedPersons(persons);
        }

        private List<ApplicationMessage> ProcessViculatedPersons(List<Person> persons)
        {
            List<ApplicationMessage> processMessages = new List<ApplicationMessage>();

            int rowCont = 1;
            foreach (Person person in persons)
            {
                ApplicationMessage errorMessage;

                List<ValidationResult> validationResults = new List<ValidationResult>();

                Customer customer = this.customerBussines.GetCustomerById(person.Code);
                if (customer != null)
                {
                    person.ExternalSystemId = customer.ExternalSystemId;

                    Validator.TryValidateObject(person, new System.ComponentModel.DataAnnotations.ValidationContext(person), validationResults, true);

                    errorMessage = this.GetPersonErroMessage(rowCont, validationResults);

                    if (errorMessage == null)
                    {
                        this.AddAsync(person);
                    }
                }
                else
                {
                    errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonCustomerNotValid, person.Code);
                }

                if (errorMessage != null)
                {
                    processMessages.Add(errorMessage);
                }
            }

            return processMessages;
        }

        private ApplicationMessage GetPersonErroMessage(int rowCont, List<ValidationResult> validationResults)
        {
            ApplicationMessage errorMessage = null;

            if (validationResults.Any())
            {
                IEnumerable<string> errorMessages = from error in validationResults
                                                    let fieldsWithErros = string.Join(',', error.MemberNames)
                                                    select string.Format("{0}: {1}", fieldsWithErros, error.ErrorMessage);

                errorMessage = new ApplicationMessage(this.cache, MessageCode.InvalidPersonRow, rowCont, string.Join('-', errorMessages));
            }

            return errorMessage;
        }
    }
}
