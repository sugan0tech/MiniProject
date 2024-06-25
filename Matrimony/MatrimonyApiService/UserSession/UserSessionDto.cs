using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MatrimonyApiService.UserSession;

public record UserSessionDto
{
    public int UserId { get; set; }
    [MaxLength(255)] [Required] [JsonIgnore] public string RefreshToken { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsValid { get; set; }
    [MaxLength(128)] [Required] public string IpAddress { get; init; }
    [MaxLength(100)] [Required] public string UserAgent { get; init; }
    [MaxLength(100)] [Required] public string DeviceType { get; init; }
}