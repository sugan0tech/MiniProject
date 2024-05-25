using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Entities;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Validations;

namespace MatrimonyApiService.Preference;

public class Preference : BaseEntity
{
    // [Key] public int PreferenceId { get; set; }

    [MaxLength(25)]
    [EnumTypeValidation(typeof(MotherTongue))]
    public required string MotherTongue { get; set; }

    [NotMapped]
    public MotherTongue MotherTongueEnum
    {
        get => Enum.Parse<MotherTongue>(MotherTongue);
        set => MotherTongue = value.ToString();
    }

    [MaxLength(25)]
    [EnumTypeValidation(typeof(Religion))]
    public required string Religion { get; set; }

    [NotMapped]
    public Religion ReligionEnum
    {
        get => Enum.Parse<Religion>(Religion);
        set => Religion = value.ToString();
    }

    [MaxLength(25)]
    [EnumTypeValidation(typeof(Education))]
    public required string Education { get; set; }

    [NotMapped]
    public Education EducationEnum
    {
        get => Enum.Parse<Education>(Education);
        set => Education = value.ToString();
    }

    [MaxLength(25)]
    [EnumTypeValidation(typeof(Occupation))]
    public required string Occupation { get; set; }

    [NotMapped]
    public Occupation OccupationEnum
    {
        get => Enum.Parse<Occupation>(Occupation);
        set => Occupation = value.ToString();
    }

    public int MinHeight { get; set; }
    public int MaxHeight { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }

    [ForeignKey("PreferenceForProfileId")] public int? PreferenceForId { get; set; }
    public Profile.Profile? PreferenceFor { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}