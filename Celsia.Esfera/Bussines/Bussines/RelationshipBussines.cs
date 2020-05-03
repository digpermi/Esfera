using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Bussines.Bussines
{
    public class RelationshipBussines : Repository<Relationship, EsferaContext>, IRelationshipBussines
    {
        public RelationshipBussines(EsferaContext context) : base(context)
        {
        }

        /// <summary>
        /// Busca todas las relaciones
        /// </summary>
        /// <returns></returns>
        public ICollection<Relationship> GetAllRelationships()
        {
            Task<List<Relationship>> task = this.GetAsync();
            return task.Result;
        }

        public Relationship GetRelationshipById(byte id)
        {
            Task<Relationship> task = this.GetAsyncByte(id);
            task.Wait();

            return task.Result;
        }

    }
}
