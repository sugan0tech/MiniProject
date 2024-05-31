using System.Diagnostics.CodeAnalysis;

namespace MatrimonyApiService.MatchRequest;

public record MatchRequestDto
{
    public int MatchId { get; init; }

    public int SentProfileId { get; init; }
    public bool ProfileOneLike { get; init; } = true;

    public int ReceivedProfileId { get; init; }
    public bool ProfileTwoLike { get; init; }

    [ExcludeFromCodeCoverage]
    public int Level { get; init; }
    public DateTime FoundAt { get; init; }
}