using Entities.Models;
using System.Collections.Generic;

namespace Portal.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        //public Customer SelectedCustomer { get; set; }
        public int Id { get; set; }
        public int Code { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string IdentificationType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool PolicyData { get; set; }
        public string System { get; set; }
        public byte? SystemId { get; set; }
        public ICollection<ExternalSystem> ExternalSystems { get; set; }
    }
}
