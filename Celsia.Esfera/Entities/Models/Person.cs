using System;
using Entities.Data;

namespace Entities.Models
{
    public partial class Person : IEntity
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Identification { get; set; }
        public int IdentificationTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? RelationshipId { get; set; }
        public int? InterestId { get; set; }
        public bool PolicyData { get; set; }
        public int? CustomerId { get; set; }
        public int? ExternalSystemId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IdentificationType IdentificationType { get; set; }
        public virtual Interest Interest { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual ExternalSystem ExternalSystem { get; set; }
    }
}
