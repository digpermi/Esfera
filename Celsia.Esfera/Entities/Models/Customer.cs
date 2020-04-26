﻿using System.Collections.Generic;
using Entities.Data;

namespace Entities.Models
{
    public partial class Customer : IEntity
    {
        public Customer()
        {
            this.Persons = new HashSet<Person>();
        }

        public int Id { get; set; }
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public int IdentificationTypeId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool PolicyData { get; set; }
        public int? ExternalSystemId { get; set; }

        public virtual IdentificationType IdentificationType { get; set; }
        public virtual ExternalSystem ExternalSystem { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
