using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Membership;

public class MembershipRepo(MatrimonyContext context): BaseRepo<Membership>(context);