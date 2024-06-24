using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.MatchRequest;

public class MatchRequest : BaseEntity
{
    // [Key] public int MatchId { get; set; }
    [ForeignKey("SentProfileId")] public int SentProfileId { get; set; }

    [ExcludeFromCodeCoverage]
    public Profile.Profile? SentProfile { get; set; }

    // Todo to proper naming and migration
    public bool ProfileOneLike { get; set; }
    [ForeignKey("ReceivedProfileId")] public int ReceivedProfileId { get; set; }
    [ExcludeFromCodeCoverage]
    public Profile.Profile? ReceivedProfile { get; set; }
    // Todo: Rejected flag
    public bool ProfileTwoLike { get; set; }
    [Range(1, 7)] public int Level { get; set; }
    public DateTime FoundAt { get; set; }
}