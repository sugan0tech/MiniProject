using MatrimonyApiService.Commons;

namespace MatrimonyApiService.User;

public class UserRepo(MatrimonyContext context): BaseRepo<User>(context);