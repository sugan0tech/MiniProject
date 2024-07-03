using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.MatchRequest;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Chat;

using System.Threading.Tasks;

public class ChatService(
    MatrimonyContext context,
    IMatchRequestService requestService,
    IBaseRepo<Profile.Profile> profileRepo,
    ILogger<ChatService> logger)
{
    public async Task<Chat> CreateChatAsync(int profileOneId, int profileTwoId)
    {
        try
        {
            await requestService.GetAcceptedMatchRequests(profileOneId);
            await profileRepo.GetById(profileOneId);
            await profileRepo.GetById(profileTwoId);
            try
            {
                await FindChatByParticipantsAsync(profileOneId, profileTwoId);
            }
            catch (EntityNotFoundException e)
            {
                var newChat = new Chat
                {
                    SenderId = profileOneId,
                    ReceiverId = profileTwoId
                };
                var chat = context.Chats.Add(newChat);

                await context.SaveChangesAsync();
                return chat.Entity;
            }

            throw new DuplicateRequestException("Chat already Exists");
        }
        catch (EntityNotFoundException e)
        {
            logger.LogError("No Match requrest found");
            throw new AuthenticationException(e.Message);
        }
    }

    public async Task<Chat?> GetChatByIdAsync(int chatId)
    {
        return await context.Chats
            .Include(c => c.Messages)
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
            .Where(c => c.SenderId.Equals(profileId) || c.ReceiverId.Equals(profileId))
            .ToListAsync();
    }

    public async Task<Chat> FindChatByParticipantsAsync(int profileOneId, int profileTwoId)
    {
        var chat = await context.Chats
            .FirstOrDefaultAsync(c => (c.SenderId.Equals(profileOneId) || c.SenderId.Equals(profileTwoId)) &&
                                      (c.ReceiverId.Equals(profileTwoId) ||
                                       c.ReceiverId.Equals(profileOneId))); // Ensure exactly two participants
        return chat ?? throw new EntityNotFoundException("No chat's found");
    }

    public async Task<List<Message.Message>> GetMessagesByChatIdAsync(int chatId)
    {
        return await context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}