using MediatR;

namespace MatrimonyApiService.Membership;

public class CreateMembershipCommandHandler(IMembershipService membershipService)
    : IRequestHandler<CreateMembershipCommand, MembershipDto>
{
    public async Task<MembershipDto> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
    {
        return await membershipService.Add(request.MembershipDto);
    }
}
