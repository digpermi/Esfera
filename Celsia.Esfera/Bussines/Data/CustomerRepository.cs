using Entities.Models;

namespace Bussines.Data
{
    internal class CustomerRepository : Repository<Customer, EsferaContext>
    {
        public CustomerRepository(EsferaContext context) : base(context)
        {

        }
    }
}
