using MatrimonyApiService.Profile;
using MediatR;

namespace MatrimonyApiService.Membership;

public class DeleteMembershipCommandHandler(IMembershipService membershipService)
    : IRequestHandler<DeleteMembershipCommand, MembershipDto>
{
    public async Task<MembershipDto> Handle(DeleteMembershipCommand request, CancellationToken cancellationToken)
    {
        return await membershipService.DeleteById(request.membershipId);
    }
}