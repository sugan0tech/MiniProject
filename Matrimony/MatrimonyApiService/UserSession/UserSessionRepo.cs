using MatrimonyApiService.Commons;

namespace MatrimonyApiService.UserSession;

public class UserSessionRepo(MatrimonyContext context) : BaseRepo<UserSession>(context);