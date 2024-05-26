using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;

namespace MatrimonyApiService.Membership;

public class Membership : BaseEntity
{
    // [Key] public int MembershipId { get; set; }
    [MaxLength(20)] public required string Type { get; set; }

    [NotMapped]
    public MemberShip TypeEnum
    {
        get => Enum.Parse<MemberShip>(Type);
        set => Type = value.ToString();
    }

    [ForeignKey("ProfileId")] public int ProfileId { get; set; }
    public Profile.Profile? Profile { get; set; }

    [MaxLength(100)] public string? Description { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsTrail { get; set; }
    public bool IsTrailEnded { get; set; }
}