using System.Collections.Generic;
using Entities.Models;

namespace Bussines.Bussines
{
    public interface IPersonBussines
    {
        ICollection<Person> GetAllPersonsNoVinculed();

        ICollection<Person> GetAllPersonsVinculed(int customerId);

        void UploadVinculatedPersons(string fileName);

        Person GetPersonById(int Id);

        Person AddAsync(Person person);

        Person EditAsync(Person person);

        Person DeleteAsync(int id);

    }
}