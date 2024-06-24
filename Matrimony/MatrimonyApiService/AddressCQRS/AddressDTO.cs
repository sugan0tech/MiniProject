namespace MatrimonyApiService.AddressCQRS;

public record AddressDto
{
    public int AddressId { get; init; }
    public int UserId { get; set; }
    public string? Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
}