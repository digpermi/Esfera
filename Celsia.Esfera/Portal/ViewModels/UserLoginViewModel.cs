using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class UserLoginViewModel : BaseViewModel
    {

        public ApplicationUser UsuarioLogin { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}