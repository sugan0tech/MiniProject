using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Membership;

public class MembershipRepo(MatrimonyContext context): BaseRepo<Membership>(context);