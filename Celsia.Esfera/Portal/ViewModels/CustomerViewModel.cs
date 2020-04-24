using System.Collections.Generic;
using Entities;

namespace Portal.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public Customer SelectedCustomer { get; set; }

        public IEnumerable<CustomerPerson> CustomerAsociatedPerson { get; set; }
    }
}
