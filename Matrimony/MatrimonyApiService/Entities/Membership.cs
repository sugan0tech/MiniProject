using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.Entities;

public class Membership
{
    [Key] public int MembershipId { get; set; }
    [MaxLength(20)] public string Type { get; set; }
    [MaxLength(100)] public string Description { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsTrail { get; set; }
}