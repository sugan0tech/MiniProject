using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Membership;
using MediatR;

namespace MatrimonyApiService.Profile;

public class CreateProfileCommandHandler(IProfileService profileService, IMediator mediator)
    : IRequestHandler<CreateProfileCommand, ProfileDto>
{
    public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await profileService.AddProfile(request.ProfileDto);
        
        // Create free tier membership
        var membershipDto = new MembershipDto
        {
            ProfileId = profile.ProfileId,
            Type = MemberShip.FreeUser.ToString(),
            EndsAt = DateTime.Now.AddDays(30),
            IsTrail = false,
            IsTrailEnded = false
        };
        
        var value = await mediator.Send(new CreateMembershipCommand(membershipDto));
        profile.MembershipId = value.MembershipId;

        return profile;
    }
}
