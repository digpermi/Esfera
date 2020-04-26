using Entities.Models;
using System;

namespace Portal.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Identification { get; set; }
        public string IdentificationType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public string Relationship { get; set; }
        public string Interest { get; set; }
        public bool PolicyData { get; set; }
        public string System { get; set; }
    }
}
