using Entities.Models;

namespace Bussines.Data
{
    internal class RelationshipRepository : Repository<Relationship, EsferaContext>
    {
        public RelationshipRepository(EsferaContext context) : base(context)
        {

        }
    }
}