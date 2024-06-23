using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Membership.Commands;
using MatrimonyApiService.Preference;
using MatrimonyApiService.Preference.Commands;
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

            // Creating membership
            var membershipDto = new MembershipDto
            {
                ProfileId = profile.ProfileId,
                Type = MemberShip.FreeUser.ToString(),
                EndsAt = DateTime.Now.AddDays(30),
                IsTrail = false,
                IsTrailEnded = false
            };

            var membership = await mediator.Send(new CreateMembershipCommand(membershipDto));

            // Creating Preference
            var preferenceDto = new PreferenceDto
            {
                PreferenceForId = profile.ProfileId,
                Gender = profile.Gender.Equals(Gender.Male.ToString())
                    ? Gender.Female.ToString()
                    : Gender.Male.ToString(),
                MotherTongue = profile.MotherTongue,
                Religion = profile.Religion,
                Education = profile.Education,
                Occupation = profile.Occupation,
                MinHeight = profile.Height - 1,
                MaxHeight = profile.Height + 1,
                MinAge = profile.Age - 5,
                MaxAge = profile.Age + 5,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var preference = await mediator.Send(new CreatePreferenceCommand(preferenceDto));

            profile.MembershipId = membership.MembershipId;
            profile.PreferenceId = preference.PreferenceId;
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