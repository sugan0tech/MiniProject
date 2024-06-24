using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.MatchRequest;

public class MatchRequest : BaseEntity
{
    [ForeignKey("SentProfileId")] public int SentProfileId { get; set; }

    [ExcludeFromCodeCoverage] public Profile.Profile? SentProfile { get; set; }

    [ForeignKey("ReceivedProfileId")] public int ReceivedProfileId { get; set; }
    [ExcludeFromCodeCoverage] public Profile.Profile? ReceivedProfile { get; set; }
    public bool IsRejected { get; set; }
    public bool ReceiverLike { get; set; }
    [Range(1, 7)] public int Level { get; set; }
    public DateTime FoundAt { get; set; }
}