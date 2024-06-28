using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MatrimonyApiService.Commons;
using Newtonsoft.Json;

namespace MatrimonyApiService.Chat;

public class Chat : BaseEntity
{
    public string? Name { get; set; } // to be used for groups

    [ForeignKey("SenderId")] public int SenderId { get; set; }
    [ExcludeFromCodeCoverage] [JsonIgnore] public Profile.Profile? Sender { get; set; }
    
    [ForeignKey("ReceiverId")] public int ReceiverId { get; set; }
    [ExcludeFromCodeCoverage] [JsonIgnore] public Profile.Profile? Receiver { get; set; }

    [ExcludeFromCodeCoverage] public List<Message.Message> Messages { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastMessageAt { get; set; }
}