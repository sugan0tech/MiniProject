using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Message;

public class Message : BaseEntity
{
    [ForeignKey("SenderId")] public int SenderId { get; set; }
    [ExcludeFromCodeCoverage] public Profile.Profile? Sender { get; set; }
    [ForeignKey("ReceiverId")] public int ReceiverId { get; set; }
    [ExcludeFromCodeCoverage] public Profile.Profile? Receiver { get; set; }
    
    [ForeignKey("ChatId")] public int ChatId { get; set; }
    [ExcludeFromCodeCoverage] public Chat.Chat? Chat { get; set; }
    public DateTime SentAt { get; set; }
    public bool Seen { get; set; }
}