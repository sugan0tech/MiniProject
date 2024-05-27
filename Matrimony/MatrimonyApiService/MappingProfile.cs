using MatrimonyApiService.Address;
using MatrimonyApiService.Match;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.Report;
using MatrimonyApiService.User;

namespace MatrimonyApiService;

public class MappingProfile: AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Address.Address, AddressDto>()
            .ForMember(dto => dto.AddressId, act => act.MapFrom(src => src.Id));
        CreateMap<Match.Match, MatchDto>()
            .ForMember(dto => dto.MatchId, act => act.MapFrom(src => src.Id));
        CreateMap<Membership.Membership, MembershipDto>()
            .ForMember(dto => dto.MembershipId, act => act.MapFrom(src => src.Id));
        CreateMap<Message.Message, MessageDto>()
            .ForMember(dto => dto.MessageId, act => act.MapFrom(src => src.Id));
        CreateMap<Preference.Preference, PreferenceDto>()
            .ForMember(dto => dto.PreferenceId, act => act.MapFrom(src => src.Id));
        CreateMap<Profile.Profile, ProfileDto>()
            .ForMember(dto => dto.ProfileId, act => act.MapFrom(src => src.Id));
        CreateMap<Profile.Profile, ProfilePreviewDto>()
            .ForMember(dto => dto.ProfileId, act => act.MapFrom(src => src.Id));
        CreateMap<ProfileView.ProfileView, ProfileViewDto>()
            .ForMember(dto => dto.ProfileViewId, act => act.MapFrom(src => src.Id));
        // CreateMap<Staff.Staff, StaffPlainDto>();
        CreateMap<User.User, UserDto>()
            .ForMember(dto => dto.UserId, act => act.MapFrom(src => src.Id));
        CreateMap<User.User, RegisterDTO>();
        CreateMap<Report.Report, ReportDto>();
    }
    
}