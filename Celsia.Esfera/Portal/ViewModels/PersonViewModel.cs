using Entities.Models;
using System;
using System.Collections.Generic;

namespace Portal.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        public string Identification { get; set; }
        public string IdentificationType { get; set; }
        public byte IdentificationTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public DateTime? Birthdate { get; set; }
        public string Relationship { get; set; }
        public byte? RelationshipId { get; set; }
        public string Interest { get; set; }
        public byte? InterestId { get; set; }
        public bool PolicyData { get; set; }
        public string System { get; set; }
        public byte? SystemId { get; set; }

        public ICollection<ExternalSystem> ExternalSystems { get; set; }
        public ICollection<IdentificationType> IdentificationTypes { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<Relationship> Relationships { get; set; }
    }
}
