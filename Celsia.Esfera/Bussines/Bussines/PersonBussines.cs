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
        /// <param name="Id"></param>
        /// <returns></returns>
        public ICollection<Person> GetAllPersons()
        {
            Task<List<Person>> task = this.repository.GetAsync(includeProperties : "Customer,Relationship,Interest,IdentificationType,ExternalSystem");
            return task.Result;
        }

    }
}
