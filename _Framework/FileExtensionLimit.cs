using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _Framework
{
    public class FileExtensionLimit : ValidationAttribute, IClientModelValidator
    {
        public string[] Extensions { get; set; }

        public FileExtensionLimit(string[] extensions)
        {
            Extensions = extensions;
        }

        public override bool IsValid(object value)
        {
            var file = (IFormFile)value;

            if (file == null) return true;

            var exe = Path.GetExtension(file.FileName);

            return Extensions.Contains(exe);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context.Attributes.All(x => x.Key != "data-val"))
                context.Attributes.Add("data-val", "true");

            context.Attributes.Add("data-val-extensions", string.Join(",", Extensions));
            context.Attributes.Add("data-val-fileExtensionLimit", ErrorMessage);
        }
    }
}