using MatrimonyApiService.Commons;
using MatrimonyApiService.Repos;

namespace MatrimonyApiService.Message;

public class MessageRepo(MatrimonyContext context): BaseRepo<Message>(context);