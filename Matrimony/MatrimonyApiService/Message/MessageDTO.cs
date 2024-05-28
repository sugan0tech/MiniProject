using MatrimonyApiService.User;

namespace MatrimonyApiService.Message;

public record MessageDto
{
    public int MessageId { get; init; }
    public int SenderId { get; init; }
    public int ReceiverId { get; init; }
    public DateTime SentAt { get; init; }
    public bool Seen { get; init; }
}