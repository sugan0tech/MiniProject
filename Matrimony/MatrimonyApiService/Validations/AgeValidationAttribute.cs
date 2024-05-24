using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Validations;

public class AgeValidationAttribute : ValidationAttribute
{
    private readonly int _minAge;

    public AgeValidationAttribute(int minAge)
    {
        _minAge = minAge;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int age)
        {
            if (age < _minAge)
                return new ValidationResult($"Age must be above {_minAge}");
        }

        return ValidationResult.Success;
    }
}