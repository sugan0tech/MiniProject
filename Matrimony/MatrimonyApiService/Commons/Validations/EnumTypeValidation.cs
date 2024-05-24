using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Validations;

public class EnumTypeValidation(Type enumType) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult($"Failed to parse as it is null for {enumType.Name}");

        return Enum.IsDefined(enumType, value)
            ? ValidationResult.Success
            : new ValidationResult($"Failed to parse {value} as {enumType.Name}");
    }
}