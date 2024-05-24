using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Profile;

public class ProfileRepo(MatrimonyContext context): BaseRepo<Profile>(context);