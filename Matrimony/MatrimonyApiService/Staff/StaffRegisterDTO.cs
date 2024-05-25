#nullable disable
using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Enums;

namespace MatrimonyApiService.Staff;

public record StaffRegisterDto
{
    [Required]
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    [MaxLength(256)]
    public string Email { get; init; }

    [Required] [MaxLength(50)] public string FirstName { get; init; }

    [Required] [MaxLength(50)] public string LastName { get; init; }

    [Required]
    [StringLength(10, ErrorMessage = "Phone number must be of 10 numbers")]
    public string PhoneNumber { get; init; }

    [Required] public int AddressId { get; init; }

    [Required] public string Password { get; init; }

    [Required]
    [EnumDataType(typeof(Role))]
    public Role Role { get; init; }
}