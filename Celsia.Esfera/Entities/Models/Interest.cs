using Entities.Data;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Interest : IEntity
    {
        public Interest()
        {
            this.Persons = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
