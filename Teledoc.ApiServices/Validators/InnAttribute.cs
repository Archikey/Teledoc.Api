using System.ComponentModel.DataAnnotations;
using Teledoc.ApiServices.Validators.Interfaces;

namespace Teledoc.ApiServices.Validators
{
    public class InnAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var validator = (IInnValidator)validationContext
                .GetService(typeof(IInnValidator))!;

            if (value is string inn && validator.IsValidIndividualInn(inn))
                return ValidationResult.Success;

            return new ValidationResult("Некорректный ИНН");
        }
    }
}
