using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Commons.Validations;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.User;

[Index(nameof(Email), Name = "Email_Ind", IsUnique = true)]
public class User : BaseEntity
{
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    [MaxLength(256)]
    public required string Email { get; set; }

    [MaxLength(50)] public string? FirstName { get; set; }
    [MaxLength(50)] public string? LastName { get; set; }

    [MaxLength(10, ErrorMessage = "Phone number must be of 10 numbers")]
    public string? PhoneNumber { get; set; }

    [ForeignKey("AddressId")] public int? AddressId;
    public AddressCQRS.Address? Address;
    public bool IsVerified { get; set; }
    public int LoginAttempts { get; set; }

    [Required]
    [EnumTypeValidation(typeof(Role))]
    [AllowedValues(["User", "Admin"], ErrorMessage = "Invalid Role")]
    public string Role { get; set; } = Commons.Enums.Role.User.ToString();
}