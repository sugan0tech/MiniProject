using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Message;

/// <summary>
/// Implementation of the IMessageService interface.
/// </summary>
public class MessageService(IBaseRepo<Message> repo, IMapper mapper, ILogger<MessageService> logger) : IMessageService
{
    /// <inheritdoc/>
    public async Task<MessageDto> AddMessage(MessageDto messageDto)
    {
        try
        {
            var message = mapper.Map<Message>(messageDto);
            var addedMessage = await repo.Add(message);
            return mapper.Map<MessageDto>(addedMessage);
        }
        catch (Exception ex)
        {
            logger.LogError("On AddMessage " + ex.Message);
            throw new Exception("Error adding message.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<MessageDto> GetMessageById(int id)
    {
        try
        {
            var message = await repo.GetById(id);
            return mapper.Map<MessageDto>(message);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError("On GetMessageById " + ex.Message);
            throw ;
        }
    }

    /// <inheritdoc/>
    public async Task<MessageDto> UpdateMessage(MessageDto messageDto)
    {
        try
        {
            var updatedMessage = mapper.Map<Message>(messageDto);
            updatedMessage.Id = messageDto.MessageId;
            var result = await repo.Update(updatedMessage);
            return mapper.Map<MessageDto>(result);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError("On UpdateMessage " + ex.Message);
            throw ;
        }
    }

    /// <inheritdoc/>
    public async Task<MessageDto> DeleteMessageById(int id)
    {
        try
        {
            var deletedMessage = await repo.DeleteById(id);
            return mapper.Map<MessageDto>(deletedMessage);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError("On DeleteMessageById " + ex.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<MessageDto>> GetAllMessages()
    {
        var messages = await repo.GetAll();
        return mapper.Map<List<MessageDto>>(messages);
    }

    /// <inheritdoc/>
    public async Task<List<MessageDto>> GetSentMessages(int userId)
    {
        var messages = await repo.GetAll();
        return messages.FindAll(message => message.SenderId.Equals(userId))
            .ConvertAll(mapper.Map<MessageDto>).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<MessageDto>> GetReceivedMessages(int userId)
    {
        var messages = await repo.GetAll();
        return messages.FindAll(message => message.ReceiverId.Equals(userId))
            .ConvertAll(input => mapper.Map<MessageDto>(input)).ToList();
    }
}