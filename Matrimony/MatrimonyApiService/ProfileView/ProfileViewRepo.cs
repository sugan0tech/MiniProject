using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.ProfileView;

public class ProfileViewRepo(MatrimonyContext context): BaseRepo<ProfileView>(context);