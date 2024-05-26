using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Address;

public class AddressService(IBaseRepo<Address> addressRepo, IMapper mapper) : IAddressService
{
    /// <inheritdoc/>
    public async Task<AddressDto> GetAddressById(int id)
    {
        var addressEntity = await addressRepo.GetById(id);
        return mapper.Map<AddressDto>(addressEntity);
    }

    /// <inheritdoc/>
    public async Task<List<AddressDto>> GetAllAddresses()
    {
        var addressEntities = await addressRepo.GetAll();
        var addressDtos = new List<AddressDto>();
        foreach (var addressEntity in addressEntities)
        {
            addressDtos.Add(mapper.Map<AddressDto>(addressEntity));
        }

        return addressDtos;
    }

    /// <inheritdoc/>
    public async Task<AddressDto> AddAddress(AddressDto addressDto)
    {
        var addressEntity = mapper.Map<Address>(addressDto);
        var addedAddressEntity = await addressRepo.Add(addressEntity);
        return mapper.Map<AddressDto>(addedAddressEntity);
    }

    /// <inheritdoc/>
    public async Task<AddressDto> UpdateAddress(AddressDto addressDto)
    {
        try
        {
            var updatedAddressEntity = mapper.Map<Address>(addressDto);
            updatedAddressEntity.Id = addressDto.AddressId; // Ensure the ID is set to the correct value
            var result = await addressRepo.Update(updatedAddressEntity);
            return mapper.Map<AddressDto>(result);
        }
        catch(KeyNotFoundException ex)
        {
            throw new KeyNotFoundException(ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<AddressDto> DeleteAddressById(int id)
    {
        try
        {
            var deletedAddressEntity = await addressRepo.DeleteById(id);
            return mapper.Map<AddressDto>(deletedAddressEntity);
        }
        catch (KeyNotFoundException ex)
        {
            // Log the exception
            throw;
        }
    }
}