namespace Bussines.Validators
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    public sealed class FileValidator : ValidationAttribute
    {
        public string[] Extensions { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult;
            if (value is IFormFile && this.IsFileAllowedExtension((IFormFile)value))
            {
                validationResult = ValidationResult.Success;
            }
            else
            {
                validationResult = new ValidationResult(this.ErrorMessage);
            }

            return validationResult;
        }

        private bool IsFileAllowedExtension(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            return this.Extensions.Length == 0 || this.Extensions.Contains(extension);
        }
    }
}
