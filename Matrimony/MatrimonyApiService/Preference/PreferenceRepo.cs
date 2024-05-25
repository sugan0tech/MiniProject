using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Preference;

public class PreferenceRepo(MatrimonyContext context): BaseRepo<Preference>(context);