using MediatR;

namespace MatrimonyApiService.Membership;

public class CreateMembershipCommandHandler : IRequestHandler<CreateMembershipCommand, MembershipDto>
{
    private readonly IMembershipService _membershipService;

    public CreateMembershipCommandHandler(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    public async Task<MembershipDto> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
    {
        return await _membershipService.Add(request.MembershipDto);
    }
}
