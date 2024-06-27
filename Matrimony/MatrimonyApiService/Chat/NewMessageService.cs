using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Chat;
using System.Threading.Tasks;

public class NewMessageService(MatrimonyContext context)
{

    public async Task<Message.Message> CreateMessageAsync(Message.Message message)
    {
        context.Messages.Add(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task UpdateMessageAsync(Message.Message message)
    {
        context.Messages.Update(message);
        await context.SaveChangesAsync();
    }
}
