using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Message;

public class MessageRepo(MatrimonyContext context) : BaseRepo<Message>(context);