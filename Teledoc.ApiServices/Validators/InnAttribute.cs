using System.ComponentModel.DataAnnotations;
using Teledoc.ApiServices.Validators.Interfaces;

namespace Teledoc.ApiServices.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public sealed class InnAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var validator = (IInnValidator?)validationContext.GetService(typeof(IInnValidator));

            if (value is not string inn || string.IsNullOrWhiteSpace(inn))
                return new ValidationResult("ИНН обязателен.");

            if (validator is null)
                return new ValidationResult("Сервис валидации ИНН не зарегистрирован.");

            if (validator.IsValid(inn))
                return ValidationResult.Success;

            return new ValidationResult("Некорректный ИНН.");
        }
    }
}
