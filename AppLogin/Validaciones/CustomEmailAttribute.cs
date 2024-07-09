using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppLogin.Validaciones
{
    public class CustomEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("El email es obligatorio.");
            }

            var email = value.ToString();
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!regex.IsMatch(email))
            {
                return new ValidationResult("El email no es válido.");
            }

            return ValidationResult.Success;
        }
    }
}

