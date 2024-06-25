#nullable disable
using System.Text.Json.Serialization;

namespace MatrimonyApiService.User;

public record UserDto
{
    public int UserId { get; set; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PhoneNumber { get; init; }
    public int AddressId { get; set; }
    public bool IsVerified { get; init; }

    [JsonIgnore] public byte[] Password { get; set; }
    [JsonIgnore] public byte[] HashKey { get; set; }
    public int LoginAttempts { get; init; }
    public string Role { get; init; }
}