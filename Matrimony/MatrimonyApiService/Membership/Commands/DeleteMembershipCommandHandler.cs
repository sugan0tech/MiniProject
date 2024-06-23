using MediatR;

namespace MatrimonyApiService.Membership.Commands;

public class DeleteMembershipCommandHandler(IMembershipService membershipService)
    : IRequestHandler<DeleteMembershipCommand, MembershipDto>
{
    public async Task<MembershipDto> Handle(DeleteMembershipCommand request, CancellationToken cancellationToken)
    {
        return await membershipService.DeleteById(request.membershipId);
    }
}