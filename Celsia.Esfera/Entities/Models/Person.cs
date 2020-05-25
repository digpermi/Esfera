using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public partial class Person
    {
        public int Id { get; set; }

        public int? Code { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Identificación inválida")]
        [MaxLength(10, ErrorMessage = "Identificación inválida")]
        [MinLength(8, ErrorMessage = "Identificación inválida")]
        public string Identification { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Range(1, byte.MaxValue, ErrorMessage = "Campo requerido")]
        public byte IdentificationTypeId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$", ErrorMessage = "Nombre inválido")]
        [MaxLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$", ErrorMessage = "Apellido inválido")]
        [MaxLength(30, ErrorMessage = "Máximo 30 caracteres")]
        public string LastName { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Teléfono inválido")]
        [MinLength(7, ErrorMessage = "Teléfono inválido")]
        [MaxLength(10, ErrorMessage = "Teléfono inválido")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Móvil inválido")]
        [MinLength(10, ErrorMessage = "Móvil inválido")]
        [MaxLength(10, ErrorMessage = "Móvil inválido")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [RegularExpression(@"^([a-zA-Z0-9-ZÀ-ÿ\u00f1\u00d1_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9-ZÀ-ÿ\u00f1\u00d1\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email inválido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthdate { get; set; }

        public byte? RelationshipId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Range(1, byte.MaxValue, ErrorMessage = "Campo requerido")]
        public byte InterestId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public bool PolicyData { get; set; }

        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Range(1, byte.MaxValue, ErrorMessage = "Campo requerido")]
        public byte ExternalSystemId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IdentificationType IdentificationType { get; set; }
        public virtual Interest Interest { get; set; }
        public virtual Relationship Relationship { get; set; }
        public virtual ExternalSystem ExternalSystem { get; set; }
    }
}
