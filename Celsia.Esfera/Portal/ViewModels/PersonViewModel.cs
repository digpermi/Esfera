using Entities.Models;
using System;
using System.Collections.Generic;

namespace Portal.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        public Person Person { get; set;  }

        public ICollection<ExternalSystem> ExternalSystems { get; set; }
        public ICollection<IdentificationType> IdentificationTypes { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<Relationship> Relationships { get; set; }
    }
}
