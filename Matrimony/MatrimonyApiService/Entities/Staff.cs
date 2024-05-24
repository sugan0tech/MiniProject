using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Validations;

namespace MatrimonyApiService.Entities;

public class Staff : BaseEntity
{
    // [Key] public int StaffId { get; set; }

    [Required]
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    public string Email { get; set; }

    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }

    [StringLength(10, ErrorMessage = "Phone number must be of 10 numbers")]
    public string PhoneNumber { get; set; }

    [ForeignKey("AddressId")] public int AddressId;
    public Address Address;

    public bool IsVerified { get; set; }
    public byte[] Password { get; set; }
    public byte[] HashKey { get; set; }

    [EnumTypeValidation(typeof(Role))] public string Role { get; set; }

    [NotMapped]
    public Role RoleEnum
    {
        get => Enum.Parse<Role>(Role);
        set => Role = value.ToString();
    }

    public int loginAttempts { get; set; }
}