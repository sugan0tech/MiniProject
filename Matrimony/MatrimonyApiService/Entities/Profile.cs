using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class Profile
{
    public int ProfileId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Education { get; set; }

    public int AnnualIncome { get; set; }

    // Todo Enum
    public string Occupation { get; set; }

    // Todo Enum
    public string MaritalStatus { get; set; }

    // Todo Enum
    public string MotherTongue { get; set; }

    // Todo Enum
    public string Religion { get; set; }

    // Todo Enum
    public string Ethinicity { get; set; }
    public string Bio { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public bool Habits { get; set; }

    // Todo Enum
    public string Gender { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    [ForeignKey("MembershipId")] public int MembershipId { get; set; }
    public Membership Membership { get; set; }
    [ForeignKey("ManagedById")] public int ManagedById { get; set; }
    public User ManagedBy { get; set; }
    [ForeignKey("PrimaryId")] public int UserId { get; set; }

    public User User { get; set; }

    // Todo Enum
    public string ManagedByRelation { get; set; }
    public IEnumerable<ProfileView> ProfileViews { get; set; }
    public IEnumerable<Match> Matches { get; set; }
    [ForeignKey("PreferenceId")] public int PreferenceId { get; set; }
    public Preference Preference { get; set; }
}