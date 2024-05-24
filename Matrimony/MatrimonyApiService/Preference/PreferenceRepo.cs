using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Preference;

public class PreferenceRepo(MatrimonyContext context): BaseRepo<Preference>(context);