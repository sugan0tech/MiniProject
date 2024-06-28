using MatrimonyApiService.Commons;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Chat;
using System.Threading.Tasks;

public class NewMessageService(MatrimonyContext context)
{

    public async Task<Message.Message> CreateMessageAsync(Message.Message message)
    {
        context.Messages.Add(message);
        await context.SaveChangesAsync();
        context.Entry(message).State = EntityState.Detached;
        return message;
    }

    public async Task UpdateMessageAsync(Message.Message message)
    {
        context.Messages.Update(message);
        await context.SaveChangesAsync();
    }

    public async Task<Message.Message> GetById(int id)
    {
        var msg = await context.Messages.SingleOrDefaultAsync(message => message.Id.Equals(id));
        if (msg == null)
            throw new KeyNotFoundException($"Message with key {id} not found!!!");
        context.Entry(msg).State = EntityState.Detached;
        return msg;
    }
}
