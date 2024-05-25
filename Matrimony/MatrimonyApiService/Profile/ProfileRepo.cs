using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Profile;

public class ProfileRepo(MatrimonyContext context) : BaseRepo<Profile>(context);