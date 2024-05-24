using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Validations;

namespace MatrimonyApiService.Entities;

public class Profile
{
    public int ProfileId { get; set; }

    public DateTime DateOfBirth
    {
        get => DateOfBirth;
        set
        {
            DateOfBirth = value;
            Age = DateTime.Today.Year - value.Year;
        }
    }

    [AgeValidation(21)]
    public int Age { get; set; }

    [Required(ErrorMessage = "Education is missing")]
    public string Education { get; set; }

    [NotMapped]
    public Education EducationEnum
    {
        get => Enum.Parse<Education>(Education);
        set => Education = value.ToString();
    }

    public int AnnualIncome { get; set; }

    [Required(ErrorMessage = "Occupation is missing")]
    public string Occupation { get; set; }

    [NotMapped]
    public Occupation OccupationEnum
    {
        get => Enum.Parse<Occupation>(Occupation);
        set => Occupation = value.ToString();
    }

    [Required(ErrorMessage = "MaritalStatus is missing")]
    public string MaritalStatus { get; set; }

    [NotMapped]
    public MaritalStatus MaritalStatusEnum
    {
        get => Enum.Parse<MaritalStatus>(MaritalStatus);
        set => MaritalStatus = value.ToString();
    }

    [Required(ErrorMessage = "MotherTongue is missing")]
    public string MotherTongue { get; set; }

    [NotMapped]
    public MotherTongue MotherTongueEnum
    {
        get => Enum.Parse<MotherTongue>(MotherTongue);
        set => MotherTongue = value.ToString();
    }

    [Required(ErrorMessage = "Religion is missing")]
    public string Religion { get; set; }

    [NotMapped]
    public Religion ReligionEnum
    {
        get => Enum.Parse<Religion>(Religion);
        set => Religion = value.ToString();
    }

    [Required(ErrorMessage = "Ethinicity is missing")]
    public string Ethinicity { get; set; }

    [NotMapped]
    public Ethnicity EthnicityEnum
    {
        get => Enum.Parse<Ethnicity>(Ethinicity);
        set => Ethinicity = value.ToString();
    }

    public string Bio { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public bool Habits { get; set; }

    [Required(ErrorMessage = "Gender is missing")]
    public string Gender { get; set; }

    [NotMapped]
    public Gender GenderEnum
    {
        get => Enum.Parse<Gender>(Gender);
        set => Gender = value.ToString();
    }

    public int Weight { get; set; }
    public int Height { get; set; }

    [ForeignKey("MembershipId")] public int MembershipId { get; set; }
    public Membership Membership { get; set; }

    [ForeignKey("ManagedById")] public int ManagedById { get; set; }
    public User ManagedBy { get; set; }

    [ForeignKey("PrimaryId")] public int UserId { get; set; }
    public User User { get; set; }

    public string ManagedByRelation { get; set; }

    [NotMapped]
    public Relation RelationEnum
    {
        get => Enum.Parse<Relation>(ManagedByRelation);
        set => ManagedByRelation = value.ToString();
    }

    public IEnumerable<ProfileView> ProfileViews { get; set; }
    public IEnumerable<Match> Matches { get; set; }

    [ForeignKey("PreferenceId")] public int PreferenceId { get; set; }
    public Preference Preference { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}