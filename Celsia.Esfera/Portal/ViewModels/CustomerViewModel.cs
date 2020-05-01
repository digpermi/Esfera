using Entities.Models;
using System.Collections.Generic;

namespace Portal.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public Customer Customer { get; set; }

        public ICollection<ExternalSystem> ExternalSystems { get; set; }
    }
}
