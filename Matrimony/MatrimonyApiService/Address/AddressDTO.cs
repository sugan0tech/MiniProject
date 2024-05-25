namespace MatrimonyApiService.Address;

public record AddressDto
{
    public int AddressId { get; init; }
    public string? Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
}