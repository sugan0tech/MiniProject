using MatrimonyApiService.Address;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.AddressCQRS.Query;

public class GetAllAddressesQueryHandler(IBaseRepo<Address.Address> repository)
    : IQueryHandler<GetAllAddressesQuery, List<AddressDto>>
{
    public async Task<List<AddressDto>> Handle(GetAllAddressesQuery query)
    {
        var addresses = await repository.GetAll();
        return addresses.Select(a => new AddressDto
        {
            Street = a.Street,
            City = a.City,
            State = a.State,
            Country = a.Country
        }).ToList();
    }
}