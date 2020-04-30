using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;
using Utilities.File;

namespace Bussines.Bussines
{
    public class PersonBussines : IPersonBussines
    {
        private readonly IRepository<Person> repository;
        private readonly ICustomerBussines customerBussines;

        public PersonBussines(EsferaContext context)
        {
            this.repository = new PersonRepository(context);
            this.customerBussines = new CustomerBussines(context);
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

        public void UploadVinculatedPersons(string fileName)
        {
            CsvFile<Person> csvFile = new CsvFile<Person>(new CsvPersonMapper());

            List<Person> person = csvFile.ParseCSVFile(fileName).ToList();

            this.ProcessviculatedPersons(person);
        }

        private void ProcessviculatedPersons(List<Person> person)
        {
            foreach (Person item in person)
            {
                Customer customer = this.customerBussines.GetCustomerById(item.Code.Value);
                Person ExistPeron = this.GetPersonById(Convert.ToInt32(item.Identification));

                if (customer != null && ExistPeron == null)
                {
                    this.AddAsync(item);
                }
                else
                {
                    //error

                }
            }
        }

    }
}
