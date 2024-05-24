using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Match;

public class MatchRepo(MatrimonyContext context): BaseRepo<Match>(context);