using MediatR;

namespace MatrimonyApiService.Membership.Commands;
public record DeleteMembershipCommand(int membershipId) : IRequest<MembershipDto>;
