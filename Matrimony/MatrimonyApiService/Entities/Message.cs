using System.ComponentModel.DataAnnotations.Schema;

namespace MatrimonyApiService.Entities;

public class Message : BaseEntity
{
    // [Key] public int MessageId { get; set; }
    [ForeignKey("SenderId")] public int SenderId { get; set; }
    public User? Sender { get; set; }
    [ForeignKey("ReceiverId")] public int ReceiverId { get; set; }
    public User? Receiver { get; set; }
    public DateTime SentAt { get; set; }
    public bool Seen { get; set; }
}