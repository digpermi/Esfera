using System;

namespace Portal.Models
{
    public class Person
    {
        public int Code { get; set; }
        public string Identification { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public Relationsships Relation { get; set; }
        public Interests Interested { get; set; }
        public byte PolicyData { get; set; }
        public ExternalSystem System { get; set; }
        public int CustomerId { get; set; }
    }
}
