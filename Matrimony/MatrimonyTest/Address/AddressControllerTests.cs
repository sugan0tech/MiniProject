using AutoMapper;
using MatrimonyApiService.Address;
using MatrimonyApiService.AddressCQRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using AddressController = MatrimonyApiService.Address.AddressController;

namespace MatrimonyTest.Address;

public class AddressControllerTests
{
    private Mock<IAddressService> _addressServiceMock;
    private Mock<ILogger<AddressController>> _loggerMock;
    private Mock<IMapper> _mapperMock;
    private AddressController _addressController;

    [SetUp]
    public void SetUp()
    {
        _addressServiceMock = new Mock<IAddressService>();
        _loggerMock = new Mock<ILogger<AddressController>>();
        _mapperMock = new Mock<IMapper>();
        _addressController = new AddressController(_addressServiceMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task GetAddressById_ReturnsOk_WhenAddressExists()
    {
        // Arrange
        int addressId = 1;
        var address = new AddressDto { AddressId = addressId };
        _addressServiceMock.Setup(service => service.GetAddressById(addressId)).ReturnsAsync(address);

        // Act
        var result = await _addressController.GetAddressById(addressId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(address, result.Value);
    }

    [Test]
    public async Task GetAddressById_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        // Arrange
        int addressId = 1;
        _addressServiceMock.Setup(service => service.GetAddressById(addressId)).Throws<KeyNotFoundException>();

        // Act
        var result = await _addressController.GetAddressById(addressId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetAllAddresses_ReturnsOk_WithAddressList()
    {
        // Arrange
        var addresses = new List<AddressDto> { new AddressDto { AddressId = 1 }, new AddressDto { AddressId = 2 } };
        _addressServiceMock.Setup(service => service.GetAllAddresses()).ReturnsAsync(addresses);

        // Act
        var result = await _addressController.GetAllAddresses() as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(addresses, result.Value);
    }

    [Test]
    public async Task AddAddress_ReturnsCreated_WhenAddressIsValid()
    {
        // Arrange
        var addressDto = new AddressDto { AddressId = 1 };
        _addressServiceMock.Setup(service => service.AddAddress(addressDto)).ReturnsAsync(addressDto);

        // Act
        var result = await _addressController.AddAddress(addressDto) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        ClassicAssert.AreEqual(addressDto, result.Value);
    }

    [Test]
    public async Task UpdateAddress_ReturnsOk_WhenAddressIsValid()
    {
        // Arrange
        var addressDto = new AddressDto { AddressId = 1 };
        _addressServiceMock.Setup(service => service.UpdateAddress(addressDto)).ReturnsAsync(addressDto);

        // Act
        var result = await _addressController.UpdateAddress(addressDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(addressDto, result.Value);
    }

    [Test]
    public async Task UpdateAddress_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        // Arrange
        var addressDto = new AddressDto { AddressId = 1 };
        _addressServiceMock.Setup(service => service.UpdateAddress(addressDto)).Throws<KeyNotFoundException>();

        // Act
        var result = await _addressController.UpdateAddress(addressDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task DeleteAddressById_ReturnsOk_WhenAddressExists()
    {
        // Arrange
        int addressId = 1;
        var address = new AddressDto { AddressId = addressId };
        _addressServiceMock.Setup(service => service.DeleteAddressById(addressId)).ReturnsAsync(address);

        // Act
        var result = await _addressController.DeleteAddressById(addressId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(address, result.Value);
    }

    [Test]
    public async Task DeleteAddressById_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        // Arrange
        int addressId = 1;
        _addressServiceMock.Setup(service => service.DeleteAddressById(addressId)).Throws<KeyNotFoundException>();

        // Act
        var result = await _addressController.DeleteAddressById(addressId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }
}