using MediatR;

namespace MatrimonyApiService.Membership;

public record CreateMembershipCommand(MembershipDto MembershipDto) : IRequest<MembershipDto>;
