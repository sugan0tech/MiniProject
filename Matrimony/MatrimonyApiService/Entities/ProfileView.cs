using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class ProfileView
{
    [Key] public int Id { get; set; }
    [ForeignKey("ViewerId")] public int ViewerId { get; set; }
    public User Viewer { get; set; }
    [ForeignKey("ViewedProfileId")] public int ViewedProfileAt { get; set; }
    public Profile ViewedAtProfile { get; set; } 
    public DateTime  ViewedAt { get; set; }
    
}