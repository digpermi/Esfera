using System.Collections.Generic;
using Entities.Models;
using Utilities.Messages;

namespace Bussines.Bussines
{
    public interface IPersonBussines
    {
        ICollection<Person> GetAllPersonsNoVinculed();

        ICollection<Person> GetAllPersonsVinculed(int customerId);

        List<ApplicationMessage> UploadVinculatedPersons(string fileName);

        Person GetPersonById(int Id);

        Person GetPersonByIdentification(string identification);

        Person GetPersonByIdentificationById(string identification, int id);

        Person Add(Person person);

        Person Edit(Person person);

        Person Delete(int id);

    }
}