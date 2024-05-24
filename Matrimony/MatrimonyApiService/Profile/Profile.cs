using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Entities;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Validations;

namespace MatrimonyApiService.Profile;

public class Profile : BaseEntity
{
    // public int ProfileId { get; set; }

    public DateTime DateOfBirth
    {
        get => DateOfBirth;
        set => Age = DateTime.Today.Year - value.Year;
    }

    [AgeValidation(21)] public int Age { get; set; }

    [Required(ErrorMessage = "Education is missing")]
    [EnumTypeValidation(typeof(Education))]
    [MaxLength(20)]
    public required string Education { get; set; }

    [NotMapped]
    public Education EducationEnum
    {
        get => Enum.Parse<Education>(Education);
        set => Education = value.ToString();
    }

    public int AnnualIncome { get; set; }

    [Required(ErrorMessage = "Occupation is missing")]
    [EnumTypeValidation(typeof(Occupation))]
    [MaxLength(50)]
    public required string Occupation { get; set; }

    [NotMapped]
    public Occupation OccupationEnum
    {
        get => Enum.Parse<Occupation>(Occupation);
        set => Occupation = value.ToString();
    }

    [Required(ErrorMessage = "MaritalStatus is missing")]
    [EnumTypeValidation(typeof(MaritalStatus))]
    [MaxLength(25)]
    public required string MaritalStatus { get; set; }

    [NotMapped]
    public MaritalStatus MaritalStatusEnum
    {
        get => Enum.Parse<MaritalStatus>(MaritalStatus);
        set => MaritalStatus = value.ToString();
    }

    [Required(ErrorMessage = "MotherTongue is missing")]
    [EnumTypeValidation(typeof(MotherTongue))]
    [MaxLength(25)]
    public required string MotherTongue { get; set; }

    [NotMapped]
    public MotherTongue MotherTongueEnum
    {
        get => Enum.Parse<MotherTongue>(MotherTongue);
        set => MotherTongue = value.ToString();
    }

    [Required(ErrorMessage = "Religion is missing")]
    [EnumTypeValidation(typeof(Religion))]
    [MaxLength(25)]
    public required string Religion { get; set; }

    [NotMapped]
    public Religion ReligionEnum
    {
        get => Enum.Parse<Religion>(Religion);
        set => Religion = value.ToString();
    }

    [Required(ErrorMessage = "Ethinicity is missing")]
    [EnumTypeValidation(typeof(Ethnicity))]
    [MaxLength(25)]
    public required string Ethnicity { get; set; }

    [NotMapped]
    public Ethnicity EthnicityEnum
    {
        get => Enum.Parse<Ethnicity>(Ethnicity);
        set => Ethnicity = value.ToString();
    }

    [MaxLength(150)]
    public string? Bio { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public bool Habits { get; set; }

    [Required(ErrorMessage = "Gender is missing")]
    [EnumTypeValidation(typeof(Gender))]
    [MaxLength(15)]
    public required string Gender { get; set; }

    [NotMapped]
    public Gender GenderEnum
    {
        get => Enum.Parse<Gender>(Gender);
        set => Gender = value.ToString();
    }

    public int Weight { get; set; }
    public int Height { get; set; }

    [ForeignKey("MembershipId")] public int MembershipId { get; set; }
    public required Membership.Membership Membership { get; set; }

    [ForeignKey("ManagedById")] public int ManagedById { get; set; }
    public User.User? ManagedBy { get; set; }

    [ForeignKey("PrimaryId")] public int UserId { get; set; }
    public User.User? User { get; set; }

    [Required(ErrorMessage = "No mapping found for ManagedByRelation")]
    [EnumTypeValidation(typeof(Relation))]
    [MaxLength(25)]
    public required string ManagedByRelation { get; set; }

    [NotMapped]
    public Relation RelationEnum
    {
        get => Enum.Parse<Relation>(ManagedByRelation);
        set => ManagedByRelation = value.ToString();
    }

    public IEnumerable<ProfileView.ProfileView>? ProfileViews { get; set; }
    public IEnumerable<Match.Match>? Matches { get; set; }

    [ForeignKey("PreferenceId")] public int PreferenceId { get; set; }
    public Preference.Preference? Preference { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}