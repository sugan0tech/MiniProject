using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Entities;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Validations;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Staff;

[Index(nameof(Email), Name = "Email_Ind")]
public class Staff : BaseEntity
{
    // [Key] public int StaffId { get; set; }

    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    [MaxLength(256)]
    public required string Email { get; set; }

    [MaxLength(50)] public required string FirstName { get; set; }
    [MaxLength(50)] public required string LastName { get; set; }

    [StringLength(10, ErrorMessage = "Phone number must be of 10 numbers")]
    public required string PhoneNumber { get; set; }

    [ForeignKey("AddressId")] public int AddressId;
    public Address.Address? Address;

    public bool IsVerified { get; set; }
    public byte[]? Password { get; set; }
    public byte[]? HashKey { get; set; }

    [EnumTypeValidation(typeof(Role))]
    [MaxLength(25)]
    public required string Role { get; set; }

    [NotMapped]
    public Role RoleEnum
    {
        get => Enum.Parse<Role>(Role);
        set => Role = value.ToString();
    }

    public int LoginAttempts { get; set; } = 0;
}