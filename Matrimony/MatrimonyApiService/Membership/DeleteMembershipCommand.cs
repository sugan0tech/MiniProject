using MediatR;

namespace MatrimonyApiService.Membership;
public record DeleteMembershipCommand(int membershipId) : IRequest<MembershipDto>;
