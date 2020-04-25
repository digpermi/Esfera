using System.Collections.Generic;
using Entities;
using Portal.Models;

namespace Portal.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public Customer SelectedCustomer { get; set; }

        public IEnumerable<CustomerPerson> CustomerAsociatedPerson { get; set; }
    }
}
