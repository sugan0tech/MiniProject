namespace MatrimonyEventConsumer.Models;

public class EventPayload
{
    public string EventType { get; set; }
    public Address Address { get; set; }
    public DateTime timestamp { get; set; } = DateTime.Now;
}