using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.UserSession;

public class UserSession : BaseEntity
{
    public int UserId { get; set; }
    [MaxLength(526)] public required string RefreshToken { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsValid { get; set; }
    [MaxLength(128)] public required string IpAddress { get; set; }
    [MaxLength(100)] public required string UserAgent { get; set; }
    [MaxLength(100)] public required string DeviceType { get; set; }
}