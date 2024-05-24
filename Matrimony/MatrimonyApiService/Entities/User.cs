using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class User : BaseEntity
{
    // [Key] public int UserId { get; set; }

    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    public string Email { get; set; }

    public required string FirstName { get; set; }
    public string LastName { get; set; }

    [MaxLength(10, ErrorMessage = "Phone number must be of 10 numbers")]
    public required string PhoneNumber { get; set; }

    [ForeignKey("AddressId")] public int AddressId;
    public Address? Address;
    public bool IsVerified { get; set; }
    public byte[]? Password { get; set; }
    public byte[]? HashKey { get; set; }
    public int LoginAttempts { get; set; } = 0;
    public IEnumerable<Message>? Messages { get; set; }
}