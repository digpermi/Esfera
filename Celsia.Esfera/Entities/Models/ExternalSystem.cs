using System.Collections.Generic;

namespace Entities.Models
{
    public partial class ExternalSystem
    {
        public ExternalSystem()
        {
            this.Customers = new HashSet<Customer>();
            this.Persons = new HashSet<Person>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
