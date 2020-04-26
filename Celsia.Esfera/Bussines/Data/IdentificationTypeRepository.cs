using Entities.Models;

namespace Bussines.Data
{
    internal class IdentificationTypeRepository : Repository<IdentificationType, EsferaContext>
    {
        public IdentificationTypeRepository(EsferaContext context) : base(context)
        {

        }
    }
}