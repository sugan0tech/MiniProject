using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Membership;

public record MembershipDto
{
    public int MembershipId { get; init; }

    [Required] [MaxLength(20)] public string Type { get; init; }

    public int ProfileId { get; init; }

    [Required] [MaxLength(100)] public string Description { get; init; }

    public DateTime EndsAt { get; init; }
    public bool IsTrail { get; init; }
    public bool IsTrailEnded { get; init; }
}