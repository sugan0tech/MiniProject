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
            var existingAddressEntity = await addressRepo.GetById(addressDto.AddressId);
            if (existingAddressEntity == null)
            {
                throw new KeyNotFoundException($"Address with id {addressDto.AddressId} not found.");
            }

            var updatedAddressEntity = mapper.Map<Address>(addressDto);
            updatedAddressEntity.Id = addressDto.AddressId; // Ensure the ID is set to the correct value
            var result = await addressRepo.Update(updatedAddressEntity);
            return mapper.Map<AddressDto>(result);
        }
        catch (Exception ex)
        {
            // Log the exception
            throw new Exception($"Error updating address with id {addressDto.AddressId}.", ex);
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
        catch (Exception ex)
        {
            // Log the exception
            throw new Exception($"Error deleting address with id {id}.", ex);
        }
    }
}