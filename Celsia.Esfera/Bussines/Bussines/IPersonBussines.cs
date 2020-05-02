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

        Person AddAsync(Person person);

        Person EditAsync(Person person);

        Person DeleteAsync(int id);

    }
}