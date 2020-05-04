using System.Collections.Generic;
using Entities.Models;
using Utilities.Messages;

namespace Bussines.Bussines
{
    public interface IPersonBussines
    {
        ICollection<Person> GetAllPersonsNoVinculed();

        List<ApplicationMessage> UploadVinculatedPersons(string fileName, string userName);

        Person GetPersonById(int Id);

        Person GetPersonByIdentification(string identification);

        Person GetPersonByIdentificationById(string identification, int id);

        Person Add(Person person, string userName);

        Person Edit(Person person, string userName);

        Person Delete(int id, string userName);

    }
}