using Entities.Models;

namespace Bussines.Data
{
    internal class InterestRepository : Repository<Interest, EsferaContext>
    {
        public InterestRepository(EsferaContext context) : base(context)
        {

        }
    }
}