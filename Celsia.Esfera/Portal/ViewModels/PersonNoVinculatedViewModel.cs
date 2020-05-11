using System.Collections.Generic;
using Entities.Models;

namespace Portal.ViewModels
{
    public class PersonNoVinculatedViewModel : BaseViewModel
    {
        public List<Person> Persons { get; set; }

        public Person CurrentPerson { get; set; }

        public ICollection<ExternalSystem> ExternalSystems { get; set; }
        public ICollection<IdentificationType> IdentificationTypes { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<Relationship> Relationships { get; set; }
    }
}
