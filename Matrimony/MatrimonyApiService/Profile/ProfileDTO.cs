using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Preference;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.User;
using System.Diagnostics.CodeAnalysis;

namespace MatrimonyApiService.Profile;

public record ProfileDto
{
    public int ProfileId { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int Age { get; init; }

    [EnumTypeValidation(typeof(Education))]
    public string Education { get; init; }

    public int AnnualIncome { get; init; }

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
    public string? Bio { get; init; }
    [ExcludeFromCodeCoverage]
    public byte[]? ProfilePicture { get; init; }
    [EnumTypeValidation(typeof(Habit))]
    public string Habit { get; init; }

    [EnumTypeValidation(typeof(Gender))] public string Gender { get; init; }

    public int Weight { get; init; }
    public int Height { get; init; }
    public int MembershipId { get; init; }
    public int ManagedById { get; init; }
    public int UserId { get; init; }
    public string ManagedByRelation { get; init; }
    [ExcludeFromCodeCoverage]
    public IEnumerable<ProfileViewDto>? ProfileViews { get; init; }
    [ExcludeFromCodeCoverage]
    public IEnumerable<MatchRequestDto>? SentMatches { get; init; }
    [ExcludeFromCodeCoverage]
    public IEnumerable<MatchRequestDto>? ReceivedMatches { get; init; }
    public int PreferenceId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}