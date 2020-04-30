using Entities.Models;
using TinyCsvParser.Mapping;

namespace Utilities.File
{
   public class CsvPersonMapper:CsvMapping<Person>
    {
        public CsvPersonMapper():base()
        {
            MapProperty(0, x => x.Code);
            MapProperty(1, x => x.Identification);
            MapProperty(2, x => x.IdentificationTypeId);
            MapProperty(3, x => x.LastName);
            MapProperty(4, x => x.FirstName);
            MapProperty(5, x => x.PhoneNumber);
            MapProperty(6, x => x.MobileNumber);
            MapProperty(7, x => x.RelationshipId);
            MapProperty(8, x => x.Email);
            MapProperty(9, x => x.Birthdate);
            MapProperty(10, x => x.InterestId);
        }
    }
}
