namespace MatrimonyApiService.AddressCQRS.Event;

public class AddressCreatedEvent
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Country { get; set; }
}