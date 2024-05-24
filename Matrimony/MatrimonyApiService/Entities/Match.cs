using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class Match : BaseEntity
{
    // [Key] public int MatchId { get; set; }
    [ForeignKey("ProfileOneId")] public int ProfileOneId { get; set; }
    public Profile ProfileOne { get; set; }
    public bool? ProfileOneLike { get; set; }
    [ForeignKey("ProfileTwoId")] public int ProfileTwoId { get; set; }
    public Profile ProfileTwo { get; set; }
    public bool? ProfileTwoLike { get; set; }
    [Range(1, 7)] public int Level { get; set; }
    public DateTime FoundAt { get; set; }
}