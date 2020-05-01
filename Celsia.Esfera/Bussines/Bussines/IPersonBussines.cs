using System.Collections.Generic;
using Entities.Models;
using FluentValidation;

namespace Bussines.Bussines
{
    public interface IPersonBussines
    {
        ICollection<Person> GetAllPersonsNoVinculed();

        ICollection<Person> GetAllPersonsVinculed(int customerId);

        void UploadVinculatedPersons(string fileName, IValidator validator);

        Person GetPersonById(int Id);

        Person GetPersonByIdentification(int identificationd);

        Person AddAsync(Person person);

        Person EditAsync(Person person);

        Person DeleteAsync(int id);

    }
}