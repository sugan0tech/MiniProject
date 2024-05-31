using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Membership;

public record MembershipDto
{
    public int MembershipId { get; init; }

    [Required] [MaxLength(20)] public required string Type { get; init; }

    public int ProfileId { get; init; }

    [Required] [MaxLength(100)] public string? Description { get; init; }

    public DateTime EndsAt { get; set; }
    public bool IsTrail { get; init; }
    public bool IsTrailEnded { get; init; }
    public int ViewsCount { get; set; }
    public int ChatCount { get; set; }
    public int RequestCount { get; set; }
    public int ViewersViewCount { get; set; }
}