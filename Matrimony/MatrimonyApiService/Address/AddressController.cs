using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MatrimonyApiService.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Address;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController(IAddressService addressService, ILogger<AddressController> logger, IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAddressById(int id)
    {
        try
        {
            var address = await addressService.GetAddressById(id);
            return Ok(address);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            return NotFound(new ErrorModel(404, e.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AddressDto>), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAllAddresses()
    {
        var addresses = await addressService.GetAllAddresses();
        return Ok(addresses);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAddress([FromBody] AddressDto addressDto)
    {
        logger.LogInformation(addressDto.ToString());
        var createdAddress = await addressService.AddAddress(addressDto);
        return StatusCode(201, createdAddress);
    }

    [HttpPut]
    [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAddress([FromBody] AddressDto addressDto)
    {
        try
        {
            var updatedAddress = await addressService.UpdateAddress(addressDto);
            return Ok(updatedAddress);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(404, ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Ok<AddressDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAddressById(int id)
    {
        try
        {
            var deletedAddress = await addressService.DeleteAddressById(id);
            return Ok(deletedAddress);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            return NotFound(new ErrorModel(404, e.Message));
        }
    }
}