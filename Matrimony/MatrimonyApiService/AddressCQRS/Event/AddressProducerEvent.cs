namespace MatrimonyApiService.AddressCQRS.Event;

public class AddressProducerEvent
{
    public int AddressId { get; set; }
    public EventPayload EventPayload { get; set; }
}