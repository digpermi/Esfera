using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bussines.Data;
using Entities.Models;

namespace Bussines.Bussines
{
    public class RelationshipBussines : IRelationshipBussines
    {
        private readonly IRepository<Relationship> repository;

        public RelationshipBussines(EsferaContext context)
        {
            this.repository = new RelationshipRepository(context);
        }

        /// <summary>
        /// Busca todas las relaciones
        /// </summary>
        /// <returns></returns>
        public ICollection<Relationship> GetAllRelationships()
        {
            Task<List<Relationship>> task = this.repository.GetAsync();
            return task.Result;
        }

        public Relationship GetRelationshipById(byte id)
        {
            Task<Relationship> task = this.repository.GetAsync(id);
            task.Wait();

            return task.Result;
        }

    }
}
