using System;

namespace Entities.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Identification { get; set; }
        public byte IdentificationType { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellNumber { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public byte? Relation { get; set; }
        public byte? Interested { get; set; }
        public bool PolicyData { get; set; }
        public int? CustomerId { get; set; }
        public byte? System { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IdentificationType IdentificationTypeNavigation { get; set; }
        public virtual Interest InterestedNavigation { get; set; }
        public virtual Relationsship RelationNavigation { get; set; }
        public virtual ExternalSystem SystemNavigation { get; set; }
    }
}
