namespace MatrimonyApiService.Message;

/// <summary>
/// Interface for message service.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Adds a new message.
    /// </summary>
    /// <param name="messageDto">The message data transfer object.</param>
    /// <returns>The added message DTO.</returns>
    Task<MessageDto> AddMessage(MessageDto messageDto);

    /// <summary>
    /// Gets a message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message.</param>
    /// <returns>The message DTO.</returns>
    Task<MessageDto> GetMessageById(int id);

    /// <summary>
    /// Updates an existing message.
    /// </summary>
    /// <param name="messageDto">The message data transfer object.</param>
    /// <returns>The updated message DTO.</returns>
    Task<MessageDto> UpdateMessage(MessageDto messageDto);

    /// <summary>
    /// Deletes a message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message.</param>
    /// <returns>The deleted message DTO.</returns>
    Task<MessageDto> DeleteMessageById(int id);

    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <returns>A list of message DTOs.</returns>
    Task<List<MessageDto>> GetAllMessages();

    /// <summary>
    /// Gets all the send messages for the user.
    /// </summary>
    /// <param name="senderId"></param>
    /// <returns></returns>
    Task<List<MessageDto>> GetSentMessages(int senderId);

    /// <summary>
    /// Gets all the received messages for the user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<MessageDto>> GetReceivedMessages(int userId);
}