using Entities.Models;

namespace Bussines.Data
{
    internal class ExternalSystemRepository : Repository<ExternalSystem, EsferaContext>
    {
        public ExternalSystemRepository(EsferaContext context) : base(context)
        {

        }
    }
}