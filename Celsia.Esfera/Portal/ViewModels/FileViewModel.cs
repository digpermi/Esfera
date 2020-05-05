using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Utilities.Messages;

namespace Portal.ViewModels
{
    public class FileViewModel : BaseViewModel
    {
        public List<ApplicationMessage> Messages { get; set; }

        public int TotalRows { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public IFormFile UploadFile { get; set; }

    }
}
