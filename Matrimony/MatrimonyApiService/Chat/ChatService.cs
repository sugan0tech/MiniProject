using MatrimonyApiService.Commons;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Chat;

using System.Threading.Tasks;

public class ChatService(MatrimonyContext context)
{
    public async Task<Chat> CreateChatAsync(Chat chat)
    {
        context.Chats.Add(chat);
        await context.SaveChangesAsync();
        return chat;
    }

    public async Task<Chat?> GetChatByIdAsync(int chatId)
    {
        return await context.Chats
            .Include(c => c.Messages)
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == chatId);
    }

    public async Task UpdateChatAsync(Chat chat)
    {
        context.Chats.Update(chat);
        await context.SaveChangesAsync();
    }

    public async Task<List<Chat>> GetChatsByUserIdAsync(int profileId)
    {
        return await context.Chats
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.Id == profileId))
            .ToListAsync();
    }

    public async Task<List<Message.Message>> GetMessagesByChatIdAsync(int chatId)
    {
        return await context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}