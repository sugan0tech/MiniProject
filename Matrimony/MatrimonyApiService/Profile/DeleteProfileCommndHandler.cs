using MatrimonyApiService.Commons;
using MatrimonyApiService.Membership;
using MediatR;

namespace MatrimonyApiService.Profile;
public class DeleteProfileCommandHandler(IProfileService profileService, IMediator mediator, MatrimonyContext context)
    : IRequestHandler<DeleteProfileCommand, ProfileDto>
{
    public async Task<ProfileDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        // Start a transaction
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var profile = await profileService.DeleteProfileById(request.profileId);

            await mediator.Send(new DeleteMembershipCommand((int) profile.MembershipId), cancellationToken);

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
