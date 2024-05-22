using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class User
{
    [Key] public int UserId { get; set; }

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
    public int loginAttempts { get; set; } = 0;
    public IEnumerable<Message> Messages { get; set; }
}