using AutoMapper;
using MatrimonyApiService.Commons;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyApiService.Address.UnitTests;

public class AddressServiceTests
{
    private Mock<IBaseRepo<Address>> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private AddressService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<Address>>();
        _mockMapper = new Mock<IMapper>();
        _service = new AddressService(_mockRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAddressById_ValidId_ReturnsAddressDto()
    {
        // Arrange
        var expectedAddressId = 1;
        var expectedAddress = new Address
            { Id = expectedAddressId, City = "Banglore", Country = "India", State = "Kerala", Street = "ring" };
        var expectedAddressDto = new AddressDto
            { AddressId = expectedAddressId, City = "Banglore", Country = "India", State = "Kerala", Street = "ring" };

        _mockRepo.Setup(repo => repo.GetById(expectedAddressId)).ReturnsAsync(expectedAddress);
        _mockMapper.Setup(mapper => mapper.Map<AddressDto>(expectedAddress)).Returns(expectedAddressDto);

        // Act
        var result = await _service.GetAddressById(expectedAddressId);

        // Assert
        ClassicAssert.AreEqual(expectedAddressDto, result);
    }

    //
    // [Test]
    // public async Task UpdateAddress_NonexistentId_ThrowsKeyNotFoundException()
    // {
    //     // Arrange
    //     var addressDto = new AddressDto {AddressId = 1, City = "Banglore", Country = "India", State = "Kerala", Street = "ring" };
    //
    //     _mockRepo.Setup(repo => repo.GetById(addressDto.AddressId)).ReturnsAsync((Address)null);
    //
    //     // Act & Assert
    //     Assert.Throws<KeyNotFoundException>(() => _service.UpdateAddress(addressDto));
    // }
    //
    // [Test]
    // public async Task UpdateAddress_ExceptionInRepo_ThrowsException()
    // {
    //     // Arrange
    //     var address = new Address { Id = 1 , City = "Banglore", Country = "India", State = "Kerala", Street = "ring"};
    //     var addressDto = new AddressDto { AddressId = 1 ,City = "Banglore", Country = "India", State = "Kerala", Street = "ring"};
    //
    //     _mockRepo.Setup(repo => repo.GetById(addressDto.AddressId)).ReturnsAsync(address);
    //     _mockRepo.Setup(repo => repo.Update(It.IsAny<Address>())).ThrowsAsync(new Exception("Test exception"));
    //
    //     // Act & Assert
    //     var exception = Assert.Throws<Exception>(() => _service.UpdateAddress(addressDto));
    //     ClassicAssert.AreEqual("Error updating address with id 1.", exception.Message);
    //     ClassicAssert.AreEqual("Test exception", exception.InnerException?.Message);
    // }
    //
}