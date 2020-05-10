using System;
using Entities.Models;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace Utilities.File
{
    public class CsvPersonMapper : CsvMapping<Person>
    {
        public CsvPersonMapper() : base()
        {
            ITypeConverter<DateTime?> dateFormat = new NullableDateTimeConverter("yyyy###MM###dd");

            this.MapProperty(0, x => x.Code);
            this.MapProperty(1, x => x.Identification);
            this.MapProperty(2, x => x.IdentificationTypeId);
            this.MapProperty(3, x => x.LastName);
            this.MapProperty(4, x => x.FirstName);
            this.MapProperty(5, x => x.PhoneNumber);
            this.MapProperty(6, x => x.MobileNumber);
            this.MapProperty(7, x => x.RelationshipId);
            this.MapProperty(8, x => x.Email);
            this.MapProperty(9, x => x.Birthdate, dateFormat);
            this.MapProperty(10, x => x.InterestId);
        }
    }
}
