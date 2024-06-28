using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Message;

public class Message : BaseEntity
{
    [ForeignKey("SenderId")] public int SenderId { get; set; }
    [ExcludeFromCodeCoverage] [JsonIgnore] public Profile.Profile? Sender { get; set; }
    [ForeignKey("ReceiverId")] public int ReceiverId { get; set; }
    [ExcludeFromCodeCoverage] [JsonIgnore] public Profile.Profile? Receiver { get; set; }
    
    [ForeignKey("ChatId")] public int ChatId { get; set; }
    [ExcludeFromCodeCoverage] [JsonIgnore] public Chat.Chat? Chat { get; set; }

    [MaxLength(512)]
    public required string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool Seen { get; set; }
}