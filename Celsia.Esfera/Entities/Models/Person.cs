using System;

namespace Entities.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        public string Identification { get; set; }
        public byte IdentificationTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public byte? RelationshipId { get; set; }
        public byte? InterestId { get; set; }
        public bool PolicyData { get; set; }
        public int? CustomerId { get; set; }
        public byte? ExternalSystemId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IdentificationType IdentificationType { get; set; }
        public virtual Interest Interest { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual ExternalSystem ExternalSystem { get; set; }
    }
}
