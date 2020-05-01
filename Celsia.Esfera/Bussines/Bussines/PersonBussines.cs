using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;
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
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly IIdentificationTypeBussines identificationTypeBussines;
        private readonly IInterestBussines interestedBussines;
        private readonly IRelationshipBussines relationshipBussines;
        private readonly ICacheUtility cache;

        public PersonBussines(EsferaContext context, ICacheUtility cache)
        {
            this.repository = new PersonRepository(context);
            this.customerBussines = new CustomerBussines(context);
            this.externalSystemBussines = new ExternalSystemBussines(context);
            this.identificationTypeBussines = new IdentificationTypeBussines(context);
            this.interestedBussines = new InterestBussines(context);
            this.relationshipBussines = new RelationshipBussines(context);
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
        public Person GetPersonById(int id)
        {
            Task<Person> task = this.repository.GetAsync(id);
            task.Wait();

            return task.Result;
        }


        /// <summary>
        /// Busca persona por identificacion
        /// </summary>
        /// <returns></returns>
        public Person GetPersonByIdentification(string identification)
        {
            Task<List<Person>> task = this.repository.GetAsync(x => x.Identification == identification);
            task.Wait();
            return task.Result.FirstOrDefault();
        }


        /// <summary>
        /// Busca persona por identificacion
        /// </summary>
        /// <returns></returns>
        public Person GetPersonByIdentificationById(string identification, int id)
        {
            Task<List<Person>> task = this.repository.GetAsync(x => x.Identification.Equals(identification) && x.Id != id);
            task.Wait();
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

        public List<ApplicationMessage> UploadVinculatedPersons(string fileName)
        {
            CsvFile<Person> csvFile = new CsvFile<Person>(new CsvPersonMapper());

            List<Person> persons = csvFile.ParseCSVFile(fileName).ToList();

            List<ApplicationMessage> processMessages = this.ProcessViculatedPersons(persons);

            return processMessages;
        }

        private List<ApplicationMessage> ProcessViculatedPersons(List<Person> persons)
        {
            List<ApplicationMessage> processMessages = new List<ApplicationMessage>();

            int rowCont = 1;
            foreach (Person person in persons)
            {
                ApplicationMessage errorMessage;

                List<ValidationResult> validationResults = new List<ValidationResult>();


                Person actualPersonId = this.GetPersonById(1);
                Person actualPerson = this.GetPersonByIdentification(person.Identification);

                if (actualPerson == null)
                {
                    IdentificationType identificationtype = this.identificationTypeBussines.GetIdentificationTypeById(person.IdentificationTypeId.Value);
                    if (identificationtype != null)
                    {
                        Relationship relationship = this.relationshipBussines.GetRelationshipById(person.RelationshipId.Value);
                        if (relationship != null)
                        {
                            Interest interest = this.interestedBussines.GetInterestById(person.InterestId.Value);
                            if (interest != null)
                            {
                                ExternalSystem externalsystem = this.externalSystemBussines.GetExternalSystemById(person.ExternalSystemId.Value);
                                if (externalsystem != null)
                                {
                                    Customer customer = this.customerBussines.GetCustomerByCode(person.Code);
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
                                }
                                else
                                {
                                    errorMessage = new ApplicationMessage(this.cache, MessageCode.ExternalSystemNotValid, person.ExternalSystemId);
                                }
                            }
                            else
                            {
                                errorMessage = new ApplicationMessage(this.cache, MessageCode.InterestNotValid, person.InterestId);
                            }
                        }
                        else
                        {
                            errorMessage = new ApplicationMessage(this.cache, MessageCode.RelationshipNotValid, person.RelationshipId);
                        }
                    }
                    else
                    {
                        errorMessage = new ApplicationMessage(this.cache, MessageCode.IdentificationTypeNotValid, person.IdentificationTypeId);
                    }
                }
                else
                {
                    errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonExist, person.Identification);
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
