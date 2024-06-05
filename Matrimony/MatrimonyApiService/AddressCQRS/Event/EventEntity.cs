using System.ComponentModel.DataAnnotations;

namespace MatrimonyApiService.AddressCQRS;

public class EventEntity
{
    [Key]
    public int EventId { get; set; }
    public string EventType { get; set; }
    public string EventData { get; set; }
    public DateTime CreatedAt { get; set; }
}
