using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Relationsship
    {
        public Relationsship()
        {
            this.Persons = new HashSet<Person>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
