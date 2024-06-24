using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Query;

public class GetAddressByIdQueryHandler(IBaseRepo<Address> repository)
    : IQueryHandler<GetAddressByIdQuery, AddressDto>
{
    public async Task<AddressDto> Handle(GetAddressByIdQuery query)
    {
        var address = await repository.GetById(query.Id);
        if (address == null) throw new KeyNotFoundException();

        return new AddressDto
        {
            UserId = address.UserId,
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country
        };
    }
}