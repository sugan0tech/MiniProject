using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.ProfileView;

public class ProfileView : BaseEntity
{
    // [Key] public int Id { get; set; }
    [ForeignKey("ViewerId")] public int ViewerId { get; set; }
    public User.User? Viewer { get; set; }
    [ForeignKey("ViewedProfileId")] public int ViewedProfileAt { get; set; }
    public Profile.Profile? ViewedAtProfile { get; set; }
    public DateTime ViewedAt { get; set; }
}