using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Message;

/// <summary>
/// Implementation of the IMessageService interface.
/// </summary>
public class MessageService(IBaseRepo<Message> repo, IMapper mapper) : IMessageService
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
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving message with id {id}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<MessageDto> UpdateMessage(MessageDto messageDto)
    {
        try
        {
            var existingMessage = await repo.GetById(messageDto.MessageId);
            if (existingMessage == null)
            {
                throw new KeyNotFoundException($"Message with id {messageDto.MessageId} not found.");
            }

            var updatedMessage = mapper.Map<Message>(messageDto);
            updatedMessage.Id = messageDto.MessageId;
            var result = await repo.Update(updatedMessage);
            return mapper.Map<MessageDto>(result);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating message with id {messageDto.MessageId}.", ex);
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
        catch (Exception ex)
        {
            throw new Exception($"Error deleting message with id {id}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<List<MessageDto>> GetAllMessages()
    {
        try
        {
            var messages = await repo.GetAll();
            return mapper.Map<List<MessageDto>>(messages);
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving all messages.", ex);
        }
    }
}