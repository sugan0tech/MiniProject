using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.ProfileView;

public class ProfileView : BaseEntity
{
    // [Key] public int Id { get; set; }
    [ForeignKey("ViewerId")] public int ViewerId { get; set; }
    [ExcludeFromCodeCoverage]
    public User.User? Viewer { get; set; }
    [ForeignKey("ViewedProfileId")] public int ViewedProfileAt { get; set; }
    [ExcludeFromCodeCoverage]
    public Profile.Profile? ViewedAtProfile { get; set; }
    public DateTime ViewedAt { get; set; }
}