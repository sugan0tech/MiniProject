using MatrimonyApiService.Profile;

namespace MatrimonyApiService.Match;

public record MatchDto
{
    public int MatchId { get; init; }

    public int SentProfileId { get; init; }
    public ProfileDto? SentProfile { get; init; }
    public bool ProfileOneLike { get; init; }

    public int ReceivedProfileId { get; init; }
    public ProfileDto? ReceivedProfile { get; init; }
    public bool ProfileTwoLike { get; init; }

    public int Level { get; init; }
    public DateTime FoundAt { get; init; }
}