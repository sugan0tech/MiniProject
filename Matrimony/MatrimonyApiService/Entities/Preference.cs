using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Enums;
using MatrimonyApiService.Validations;

namespace MatrimonyApiService.Entities;

public class Preference
{
    [Key] public int PreferenceId { get; set; }

    [MaxLength(15)]
    [EnumTypeValidation(typeof(MotherTongue))]
    public string MotherTongue { get; set; }

    [NotMapped]
    public MotherTongue MotherTongueEnum
    {
        get => Enum.Parse<MotherTongue>(MotherTongue);
        set => MotherTongue = value.ToString();
    }

    [MaxLength(15)]
    [EnumTypeValidation(typeof(Religion))]
    public string Religion { get; set; }

    [NotMapped]
    public Religion ReligionEnum
    {
        get => Enum.Parse<Religion>(Religion);
        set => Religion = value.ToString();
    }

    [MaxLength(10)]
    [EnumTypeValidation(typeof(Education))]
    public string Education { get; set; }

    [NotMapped]
    public Education EducationEnum
    {
        get => Enum.Parse<Education>(Education);
        set => Religion = value.ToString();
    }

    [MaxLength(15)]
    [EnumTypeValidation(typeof(Occupation))]
    public string Occupation { get; set; }

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

    [ForeignKey("PreferenceForProfileId")] public int PreferenceForId { get; set; }
    public Profile PreferenceFor { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}