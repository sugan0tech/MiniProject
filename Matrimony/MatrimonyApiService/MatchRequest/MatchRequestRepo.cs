using MatrimonyApiService.Commons;

namespace MatrimonyApiService.MatchRequest;

public class MatchRequestRepo(MatrimonyContext context) : BaseRepo<MatchRequest>(context);