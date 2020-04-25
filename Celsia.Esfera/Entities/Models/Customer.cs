using System.Collections.Generic;
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
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public byte IdentificationType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public bool PolicyData { get; set; }
        public byte? System { get; set; }

        public virtual IdentificationType IdentificationTypeNavigation { get; set; }
        public virtual ExternalSystem SystemNavigation { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
