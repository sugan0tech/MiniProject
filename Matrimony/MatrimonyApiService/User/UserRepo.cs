using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.User;

public class UserRepo(MatrimonyContext context): BaseRepo<User>(context);