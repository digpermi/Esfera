using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ApplicationUser
    {
        [Required(ErrorMessage = "Campo requerido")]
        public string UserName { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Document { get; set; }

        public string[] Roles { get; set; }
    }
}