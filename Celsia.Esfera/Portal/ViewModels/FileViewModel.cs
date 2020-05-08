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
        [FileExtensions(ErrorMessage = "El archivo no es válido, solo es permitido archivos CSV.", Extensions = "csv")]

        public IFormFile UploadFile { get; set; }

    }
}
