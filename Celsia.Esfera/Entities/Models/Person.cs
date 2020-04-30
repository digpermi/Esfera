using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Person
    {
        public int Id { get; set; }

        public int? Code { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string Identification { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public byte? IdentificationTypeId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthdate { get; set; }

        public byte? RelationshipId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public byte? InterestId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public bool PolicyData { get; set; }

        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public byte? ExternalSystemId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IdentificationType IdentificationType { get; set; }
        public virtual Interest Interest { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual ExternalSystem ExternalSystem { get; set; }
    }
}
