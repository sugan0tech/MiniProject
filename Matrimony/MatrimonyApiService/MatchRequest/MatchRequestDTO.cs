using System.Diagnostics.CodeAnalysis;

namespace MatrimonyApiService.MatchRequest;

public record MatchRequestDto
{
    public int MatchId { get; init; }

    public int SentProfileId { get; init; }

    public int ReceivedProfileId { get; init; }
    public bool IsRejected { get; init; } = false;
    public bool ReceiverLike { get; init; } = false;

    [ExcludeFromCodeCoverage]
    public int Level { get; init; }
    public DateTime FoundAt { get; init; }
}