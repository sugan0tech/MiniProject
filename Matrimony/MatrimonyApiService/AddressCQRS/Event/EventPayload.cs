namespace MatrimonyApiService.AddressCQRS.Event;

public class EventPayload
{
    public string EventType { get; set; }
    public Address Address { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}