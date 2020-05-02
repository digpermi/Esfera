using Entities.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Range(1, byte.MaxValue, ErrorMessage = "Campo requerido")]
        public byte ExternalSystemId { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Campo requerido")]
        public int Code { get; set; }

        public ICollection<ExternalSystem> ExternalSystems { get; set; }
    }
}
