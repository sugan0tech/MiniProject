using MediatR;

namespace MatrimonyApiService.Membership.Commands;

public record CreateMembershipCommand(MembershipDto MembershipDto) : IRequest<MembershipDto>;