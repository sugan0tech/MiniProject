using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Match;

public class MatchRepo(MatrimonyContext context) : BaseRepo<Match>(context);