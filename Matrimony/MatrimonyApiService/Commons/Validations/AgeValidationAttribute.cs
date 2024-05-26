using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Commons.Validations;

public class AgeValidationAttribute(int minAge) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int age)
        {
            if (age < minAge)
                return new ValidationResult($"Age must be above {minAge}");
        }

        return ValidationResult.Success;
    }
}