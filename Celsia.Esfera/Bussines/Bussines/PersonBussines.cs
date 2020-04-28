using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;

namespace Bussines.Bussines
{
    public class PersonBussines : IPersonBussines
    {
        private readonly IRepository<Person> repository;

        public PersonBussines(EsferaContext context)
        {
            this.repository = new PersonRepository(context);
        }

        /// <summary>
        /// Busca todas las personas
        /// </summary>
        /// <returns></returns>
        public ICollection<Person> GetAllPersons()
        {
            Task<List<Person>> task = this.repository.GetAsync(includeProperties : "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
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

    }
}
