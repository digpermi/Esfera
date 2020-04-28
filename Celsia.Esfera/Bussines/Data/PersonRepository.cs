using Entities.Models;

namespace Bussines.Data
{
    internal class PersonRepository : Repository<Person, EsferaContext>
    {
        public PersonRepository(EsferaContext context) : base(context)
        {

        }
    }
}