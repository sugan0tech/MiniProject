using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Address;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.Report;
using MatrimonyApiService.User;

namespace MatrimonyApiService;

[ExcludeFromCodeCoverage]
public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        // Address mappings
        CreateMap<Address.Address, AddressDto>()
            .ForMember(dto => dto.AddressId, act => act.MapFrom(src => src.Id));
        CreateMap<AddressDto, Address.Address>()
            .ForMember(dto => dto.Id, act => act.MapFrom(src => src.AddressId));

        // MatchRequest mappings
        CreateMap<MatchRequest.MatchRequest, MatchRequestDto>()
            .ForMember(dto => dto.MatchId, act => act.MapFrom(src => src.Id));
        CreateMap<MatchRequestDto, MatchRequest.MatchRequest>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.MatchId));

        // Membership mappings
        CreateMap<Membership.Membership, MembershipDto>()
            .ForMember(dto => dto.MembershipId, act => act.MapFrom(src => src.Id));
        CreateMap<MembershipDto, Membership.Membership>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.MembershipId));

        // Message mappings
        CreateMap<Message.Message, MessageDto>()
            .ForMember(dto => dto.MessageId, act => act.MapFrom(src => src.Id));
        CreateMap<MessageDto, Message.Message>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.MessageId));

        // Preference mappings
        CreateMap<Preference.Preference, PreferenceDto>()
            .ForMember(dto => dto.PreferenceId, act => act.MapFrom(src => src.Id));
        CreateMap<PreferenceDto, Preference.Preference>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.PreferenceId));

        // Profile mappings
        CreateMap<Profile.Profile, ProfileDto>()
            .ForMember(dto => dto.ProfileId, act => act.MapFrom(src => src.Id));
        CreateMap<ProfileDto, Profile.Profile>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.ProfileId));

        CreateMap<Profile.Profile, ProfilePreviewDto>()
            .ForMember(dto => dto.ProfileId, act => act.MapFrom(src => src.Id));
        CreateMap<ProfilePreviewDto, Profile.Profile>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.ProfileId));

        // ProfileView mappings
        CreateMap<ProfileView.ProfileView, ProfileViewDto>()
            .ForMember(dto => dto.ProfileViewId, act => act.MapFrom(src => src.Id));
        CreateMap<ProfileViewDto, ProfileView.ProfileView>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.ProfileViewId));

        // User mappings
        CreateMap<User.User, UserDto>()
            .ForMember(dto => dto.UserId, act => act.MapFrom(src => src.Id));
        CreateMap<UserDto, User.User>()
            .ForMember(entity => entity.Id, act => act.MapFrom(dto => dto.UserId));

        CreateMap<User.User, RegisterDTO>();
        CreateMap<RegisterDTO, User.User>();

        // Report mappings
        CreateMap<Report.Report, ReportDto>();
        CreateMap<ReportDto, Report.Report>();
    }
}