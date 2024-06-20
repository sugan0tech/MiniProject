using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Membership.Commands;
using MediatR;

namespace MatrimonyApiService.Profile.Commands;

public class CreateProfileCommandHandler(IProfileService profileService, IMediator mediator, MatrimonyContext context)
    : IRequestHandler<CreateProfileCommand, ProfileDto>
{
    public async Task<ProfileDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        // Start a transaction
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var profile = await profileService.AddProfile(request.ProfileDto);

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
            await profileService.UpdateProfile(profile);

            await transaction.CommitAsync(cancellationToken);

            return profile;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new Exception("Transaction failed, rolling back.", ex);
        }
    }

}
