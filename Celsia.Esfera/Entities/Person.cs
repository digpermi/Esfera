using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Person
    {
        public int Code { get; set; }
        public string Identification { get; set; }
        public int IdentificationType { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int Relation { get; set; }
        public int Interested { get; set; }
        public byte PolicyData { get; set; }
        public int System { get; set; }
        public int CustomerId { get; set; }
    }
}
