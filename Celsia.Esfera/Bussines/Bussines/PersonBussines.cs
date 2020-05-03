using System;
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
        private readonly IExternalSystemBussines externalSystemBussines;
        private readonly IIdentificationTypeBussines identificationTypeBussines;
        private readonly IInterestBussines interestedBussines;
        private readonly IRelationshipBussines relationshipBussines;
        private readonly ICacheUtility cache;
        private readonly IAuditBussines auditBussines;

        public PersonBussines(EsferaContext context, ICacheUtility cache) : base(context)
        {
            this.customerBussines = new CustomerBussines(context);
            this.externalSystemBussines = new ExternalSystemBussines(context);
            this.identificationTypeBussines = new IdentificationTypeBussines(context);
            this.interestedBussines = new InterestBussines(context);
            this.relationshipBussines = new RelationshipBussines(context);
            this.cache = cache;
            this.auditBussines = new AuditBussines(context);
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

            this.auditBussines.Add(new Audit()
            {
                dateAudit = DateTime.Now,
                usser = userName,
                operation = "Adicionar persona"
            });

           
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

            this.auditBussines.Add(new Audit
            {
                dateAudit = DateTime.Now,
                usser = userName,
                operation = "Editar persona"
            });
            
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

            this.auditBussines.Add(new Audit()
            {
                dateAudit = DateTime.Now,
                usser = userName,
                operation = "Eliminar persona"
            });

            return task.Result;
        }

        public List<ApplicationMessage> UploadVinculatedPersons(string fileName,string userName)
        {
            CsvFile<Person> csvFile = new CsvFile<Person>(new CsvPersonMapper());

            List<Person> persons = csvFile.ParseCSVFile(fileName).ToList();

            this.auditBussines.Add(new Audit()
            {
                dateAudit = DateTime.Now,
                usser = userName,
                operation = "Carga masiva"
            });

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

                Person actualPerson = this.GetPersonByIdentification(person.Identification);

                if (actualPerson == null)
                {
                    IdentificationType identificationtype = this.identificationTypeBussines.GetIdentificationTypeById(person.IdentificationTypeId);
                    if (identificationtype != null)
                    {
                        Relationship relationship = this.relationshipBussines.GetRelationshipById(person.RelationshipId.Value);
                        if (relationship != null)
                        {
                            Interest interest = this.interestedBussines.GetInterestById(person.InterestId);
                            if (interest != null)
                            {
                                Customer customer = this.customerBussines.GetCustomerByCode(person.Code);
                                if (customer != null)
                                {
                                    person.ExternalSystemId = customer.ExternalSystemId;

                                    Validator.TryValidateObject(person, new System.ComponentModel.DataAnnotations.ValidationContext(person), validationResults, true);

                                    errorMessage = this.GetPersonErroMessage(rowCont, validationResults);

                                    if (errorMessage == null)
                                    {
                                        this.Add(person);
                                        errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonImported, rowCont);
                                    }
                                }
                                else
                                {
                                    errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonCustomerNotValid, rowCont, person.Code);
                                }
                            }
                            else
                            {
                                errorMessage = new ApplicationMessage(this.cache, MessageCode.InterestNotValid, rowCont, person.InterestId);
                            }
                        }
                        else
                        {
                            errorMessage = new ApplicationMessage(this.cache, MessageCode.RelationshipNotValid, rowCont, person.RelationshipId);
                        }
                    }
                    else
                    {
                        errorMessage = new ApplicationMessage(this.cache, MessageCode.IdentificationTypeNotValid, rowCont, person.IdentificationTypeId);
                    }
                }
                else
                {
                    errorMessage = new ApplicationMessage(this.cache, MessageCode.PersonExistImport, rowCont, person.Identification);
                }

                if (errorMessage != null)
                {
                    processMessages.Add(errorMessage);
                }

                rowCont++;
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
