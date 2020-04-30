using Entities.Models;
using System.Collections.Generic;

namespace Bussines.Bussines
{
    public interface IFileBussines
    {
        bool AddPersonVinculed(List<Person> person);
    }
}
