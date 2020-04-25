
namespace Portal.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public byte PolicyData { get; set; }
        public ExternalSystem System { get; set; }
    }
}
