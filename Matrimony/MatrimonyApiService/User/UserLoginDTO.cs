#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.User;

public record UserLoginDto
{
    [Required]
    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    [MaxLength(256)]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }
}