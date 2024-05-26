using MatrimonyApiService.Address;
using MatrimonyApiService.Match;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.Staff;
using MatrimonyApiService.User;

namespace MatrimonyApiService;

public class MappingProfile: AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Address.Address, AddressDto>()
            .ForMember(dto => dto.AddressId, act => act.MapFrom(src => src.Id));
        CreateMap<Match.Match, MatchDto>();
        CreateMap<Membership.Membership, MembershipDto>();
        CreateMap<Message.Message, MessageDto>();
        CreateMap<Preference.Preference, PreferenceDto>();
        CreateMap<Profile.Profile, ProfileDto>();
        CreateMap<Profile.Profile, ProfilePreviewDto>();
        CreateMap<ProfileView.ProfileView, ProfileViewDto>();
        CreateMap<Staff.Staff, StaffPlainDto>();
        CreateMap<User.User, UserDto>();
    }
    
}