using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Commons.Validations;
using System.Diagnostics.CodeAnalysis;

namespace MatrimonyApiService.Profile;

public record ProfilePreviewDto
{
    public int ProfileId { get; init; }

    [EnumTypeValidation(typeof(Education))]
    public string Education { get; init; }

    [EnumTypeValidation(typeof(Occupation))]
    public string Occupation { get; init; }

    [EnumTypeValidation(typeof(MaritalStatus))]
    public string MaritalStatus { get; init; }

    [EnumTypeValidation(typeof(MotherTongue))]
    public string MotherTongue { get; init; }

    [EnumTypeValidation(typeof(Religion))] public string Religion { get; init; }

    [EnumTypeValidation(typeof(Ethnicity))]
    public string Ethnicity { get; init; }

    [ExcludeFromCodeCoverage]
    public byte[]? ProfilePicture { get; init; }
}