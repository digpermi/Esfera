using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Utilities.Cache;
using Utilities.File;
using Utilities.Messages;

namespace Bussines.Bussines
{
    public class PersonBussines : Repository<Person, EsferaContext>, IPersonBussines
    {
        private readonly ICustomerBussines customerBussines;
        private readonly ICacheUtility cache;

        public PersonBussines(EsferaContext context, ICacheUtility cache) : base(context)
        {
            this.customerBussines = new CustomerBussines(context);
            this.cache = cache;
        }

        /// <summary>
        /// Busca todas las personas no vinculadas
        /// </summary>
        /// <returns></returns>
        public ICollection<Person> GetAllPersonsNoVinculed()
        {
            Task<List<Person>> task = this.GetAsync(x => x.CustomerId == null, includeProperties: "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
            return task.Result;
        }

        /// <summary>
        /// Busca todas las personas no vinculadas
        /// </summary>
        /// <returns></returns>
        public ICollection<Person> GetAllPersonsVinculed(int customerId)
        {
            Task<List<Person>> task = this.GetAsync(x => x.CustomerId == customerId, includeProperties: "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
            return task.Result;
        }

        /// <summary>
        /// Busca persona por id
        /// </summary>
        /// <returns></returns>
        public Person GetPersonById(int id)
        {
            Task<Person> task = this.GetAsync(id);
            task.Wait();

            return task.Result;
        }


        /// <summary>
        /// Busca persona por identificacion
        /// </summary>
        /// <returns></returns>
        public Person GetPersonByIdentification(string identification)
        {
            Task<List<Person>> task = this.GetAsync(x => x.Identification.Equals(identification));
            task.Wait();
            return task.Result.FirstOrDefault();
        }


        /// <summary>
        /// Busca persona por identificacion
        /// </summary>
        /// <returns></returns>
        public Person GetPersonByIdentificationById(string identification, int id)
        {
            Task<List<Person>> task = this.GetAsync(x => x.Identification.Equals(identification) && x.Id != id);
            task.Wait();
            return task.Result.FirstOrDefault();
        }


        /// <summary>
        /// Inserta una persona
        /// </summary>
        /// <returns></returns>
        public Person Add(Person person)
        {
            Task<Person> task = this.AddAsync(person);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Editar una persona
        /// </summary>
        /// <returns></returns>
        public Person Edit(Person person)
        {
            Task<Person> task = this.EditAsync(person);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Eliminar una persona
        /// </summary>
        /// <returns></returns>
        public Person Delete(int Id)
        {
            Task<Person> task = this.DeleteAsync(Id);
            task.Wait();
            return task.Result;
        }

        public void UploadVinculatedPersons(string fileName)
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

                Person actualPerson = this.GetPersonByIdentification(person.Identification);
                //validar si la persona existe y mostrar el mensaje


                Customer customer = this.customerBussines.GetCustomerByCode(person.Code);
                if (customer != null)
                {
                    person.ExternalSystemId = customer.ExternalSystemId;

                    Validator.TryValidateObject(person, new System.ComponentModel.DataAnnotations.ValidationContext(person), validationResults, true);

                    errorMessage = this.GetPersonErroMessage(rowCont, validationResults);

                    if (errorMessage == null)
                    {
                        this.Add(person);
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
