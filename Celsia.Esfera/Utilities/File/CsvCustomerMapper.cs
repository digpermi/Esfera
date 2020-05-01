using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TinyCsvParser.Mapping;

namespace Utilities.File
{
   public class CsvCustomerMapper:CsvMapping<Customer>
    {
        public CsvCustomerMapper():base()
        {
            MapProperty(0, x => x.FirstName);
            MapProperty(1, x => x.LastName);
            MapProperty(2, x => x.Address);
            MapProperty(3, x => x.PhoneNumber);
        }
    }
}
