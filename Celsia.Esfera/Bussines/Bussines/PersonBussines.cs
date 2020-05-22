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
        private readonly IMasterBussinesManager masterBussinesManager;
        private readonly ICacheUtility cache;
        private readonly IAuditBussines auditBussines;

        public PersonBussines(EsferaContext context, ICacheUtility cache) : base(context)
        {
            this.customerBussines = new CustomerBussines(context);
            this.masterBussinesManager = new MasterBussinesManager(context);
            this.cache = cache;
            this.auditBussines = new AuditBussines(context);
        }

        /// <summary>
        /// Busca todas las personas no vinculadas
        /// </summary>
        /// <returns></returns>
        public List<Person> GetAllPersonsNoVinculed()
        {
            Task<List<Person>> task = this.GetAsync(x => x.CustomerId == null, includeProperties: "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
            task.Wait();
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
            Task<List<Person>> task = this.GetAsync(x => x.Identification == identification);
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
        public Person Add(Person person, string userName)
        {
            Task<Person> task = this.AddAsync(person);
            task.Wait();

            this.auditBussines.Add(userName, OperationAudit.AddPerson);

            return task.Result;
        }

        /// <summary>
        /// Editar una persona
        /// </summary>
        /// <returns></returns>
        public Person Edit(Person person, string userName)
        {
            Task<Person> task = this.EditAsync(person);
            task.Wait();

            this.auditBussines.Add(userName, OperationAudit.EditPerson);

            return task.Result;
        }

        /// <summary>
        /// Eliminar una persona
        /// </summary>
        /// <returns></returns>
        public Person Delete(int Id, string userName)
        {
            Task<Person> task = this.DeleteAsync(Id);
            task.Wait();

            this.auditBussines.Add(userName, OperationAudit.DeletePerson);

            return task.Result;
        }

        public List<ApplicationMessage> UploadVinculatedPersons(string fileName, string userName)
        {
            CsvFile<Person> csvFile = new CsvFile<Person>(new CsvPersonMapper());

            List<Person> persons = csvFile.ParseCSVFile(fileName).ToList();

            this.auditBussines.Add(userName, OperationAudit.UploadPerson);

            List<ApplicationMessage> processMessages = new List<ApplicationMessage>();

            ApplicationMessage errorMessage = this.GetPersonErroMessage(csvFile.Errors.ToList());
            if (errorMessage != null)
            {
                processMessages.Add(errorMessage);
            }
            else
            {
                processMessages = this.ProcessViculatedPersons(persons);
            }

            return processMessages;
        }



        private List<ApplicationMessage> ProcessViculatedPersons(List<Person> persons)
        {
            List<ApplicationMessage> processMessages = new List<ApplicationMessage>();

            int rowCont = 1;
            foreach (Person person in persons)
            {
                ApplicationMessage errorMessage = this.ValidateUploadPerson(person, rowCont);

                if (errorMessage == null)
                {
                    Customer customer = this.customerBussines.GetCustomerByCode(person.Code);
                    if (customer != null)
                    {
                        person.ExternalSystemId = customer.ExternalSystemId;
                        person.CustomerId = customer.Id;
                        errorMessage = this.ValidateUploadPersonModel(person, rowCont);

                        if (errorMessage == null)
                        {
                            this.AddAsync(person);
                            errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonImported, rowCont);
                        }
                    }
                    else
                    {
                        errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonCustomerNotValid, rowCont, person.Code);
                    }
                }

                processMessages.Add(errorMessage);


                rowCont++;
            }

            return processMessages;
        }

        private ApplicationMessage ValidateUploadPerson(Person person, int rowCont)
        {
            ApplicationMessage errorMessage = null;

            IdentificationType identificationtype = this.masterBussinesManager.IdentificationTypeBussines.GetIdentificationTypeById(person.IdentificationTypeId);
            Relationship relationship = this.masterBussinesManager.RelationshipBussines.GetRelationshipById(person.RelationshipId.Value);
            Interest interest = this.masterBussinesManager.InterestBussines.GetInterestById(person.InterestId);

            if (identificationtype == null)
            {
                errorMessage = new ApplicationMessage(this.cache, MessageCode.IdentificationTypeNotValid, rowCont, person.IdentificationTypeId);
            }
            else if (relationship == null)
            {
                errorMessage = new ApplicationMessage(this.cache, MessageCode.RelationshipNotValid, rowCont, person.RelationshipId);
            }
            else if (interest == null)
            {
                errorMessage = new ApplicationMessage(this.cache, MessageCode.InterestNotValid, rowCont, person.InterestId);
            }

            return errorMessage;
        }

        private ApplicationMessage ValidateUploadPersonModel(Person person, int rowCont)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(person, new ValidationContext(person), validationResults, true);

            return this.GetPersonErroMessage(rowCont, validationResults);
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

        private ApplicationMessage GetPersonErroMessage(List<string> errors)
        {
            ApplicationMessage errorMessage = null;

            if (errors.Any())
            {
                errorMessage = new ApplicationMessage(this.cache, MessageCode.ExcelUploadError, string.Join(',', errors));
            }

            return errorMessage;
        }
    }
}
