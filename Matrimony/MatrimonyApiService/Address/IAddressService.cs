namespace MatrimonyApiService.Address;

public interface IAddressService
{
    /// <summary>
    /// Retrieves an address by its unique identifier asynchronously.
    /// </summary>
    Task<AddressDto> GetAddressById(int id);

    /// <summary>
    /// Retrieves all addresses asynchronously.
    /// </summary>
    Task<List<AddressDto>> GetAllAddresses();

    /// <summary>
    /// Adds a new address asynchronously.
    /// </summary>
    Task<AddressDto> AddAddress(AddressDto addressDto);

    /// <summary>
    /// Updates an existing address asynchronously.
    /// </summary>
    Task<AddressDto> UpdateAddress(AddressDto addressDto);

    /// <summary>
    /// Deletes an address by its unique identifier asynchronously.
    /// </summary>
    Task<AddressDto> DeleteAddressById(int id);
}