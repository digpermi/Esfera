using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IPersonBussines
    {
        ICollection<Person> GetAllPersons();

        Person GetPersonById(int Id);

        Person AddAsync(Person person);

        Person EditAsync(Person person);

        Person DeleteAsync(int id);

    }
}