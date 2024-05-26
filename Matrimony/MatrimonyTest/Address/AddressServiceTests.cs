using AutoMapper;
using MatrimonyApiService.Address;
using MatrimonyApiService.Commons;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Address;

public class AddressServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Address.Address>> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private AddressService _addressService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.Address.Address>>();
        _mockMapper = new Mock<IMapper>();
        _addressService = new AddressService(_mockRepo.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAddressById_ValidId_ReturnsAddressDto()
    {
        // Arrange
        var expectedAddressId = 1;
        var expectedAddress = new MatrimonyApiService.Address.Address
            { Id = expectedAddressId, City = "Banglore", Country = "India", State = "Kerala", Street = "ring" };
        var expectedAddressDto = new AddressDto
            { AddressId = expectedAddressId, City = "Banglore", Country = "India", State = "Kerala", Street = "ring" };

        _mockRepo.Setup(repo => repo.GetById(expectedAddressId)).ReturnsAsync(expectedAddress);
        _mockMapper.Setup(mapper => mapper.Map<AddressDto>(expectedAddress)).Returns(expectedAddressDto);

        // Act
        var result = await _addressService.GetAddressById(expectedAddressId);

        // ClassicAssert
        ClassicAssert.AreEqual(expectedAddressDto, result);
    }


    [Test]
    public async Task GetAllAddresses_ShouldReturnListOfAddressDto()
    {
        // Arrange
        var addressEntities = new List<MatrimonyApiService.Address.Address>
        {
            new() { Id = 1, City = "City 1", Country = "", State = "" },
            new() { Id = 2, City = "City 2", Country = "", State = "" }
        };
        var addressDtos = new List<AddressDto>
        {
            new() { AddressId = 1, City = "City 1", Country = "", State = "" },
            new() { AddressId = 2, City = "City 2", Country = "", State = "" }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(addressEntities);
        _mockMapper.Setup(mapper => mapper.Map<AddressDto>(It.IsAny<MatrimonyApiService.Address.Address>()))
            .Returns((MatrimonyApiService.Address.Address source) =>
                new AddressDto { AddressId = source.Id, City = source.City });

        // Act
        var result = await _addressService.GetAllAddresses();

        // ClassicAssert
        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.AreEqual(1, result[0].AddressId);
        ClassicAssert.AreEqual("City 1", result[0].City);
        ClassicAssert.AreEqual(2, result[1].AddressId);
        ClassicAssert.AreEqual("City 2", result[1].City);
    }

    [Test]
    public async Task AddAddress_ShouldReturnAddedAddressDto()
    {
        // Arrange
        var addressDto = new AddressDto { AddressId = 1, City = "New City", State = "", Country = "" };
        var addressEntity = new MatrimonyApiService.Address.Address
            { Id = 1, City = "New City", State = "", Country = "" };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Address.Address>(addressDto)).Returns(addressEntity);
        _mockRepo.Setup(repo => repo.Add(addressEntity)).ReturnsAsync(addressEntity);
        _mockMapper.Setup(mapper => mapper.Map<AddressDto>(addressEntity)).Returns(addressDto);

        // Act
        var result = await _addressService.AddAddress(addressDto);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(1, result.AddressId);
        ClassicAssert.AreEqual("New City", result.City);
    }

    [Test]
    public async Task UpdateAddress_ShouldReturnUpdatedAddressDto_WhenAddressExists()
    {
        // Arrange
        var addressDto = new AddressDto { AddressId = 1, City = "Updated City" };
        var addressEntity = new MatrimonyApiService.Address.Address
            { Id = 1, City = "Updated City", State = "", Country = "" };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Address.Address>(addressDto)).Returns(addressEntity);
        _mockRepo.Setup(repo => repo.Update(addressEntity)).ReturnsAsync(addressEntity);
        _mockMapper.Setup(mapper => mapper.Map<AddressDto>(addressEntity)).Returns(addressDto);

        // Act
        var result = await _addressService.UpdateAddress(addressDto);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(1, result.AddressId);
        ClassicAssert.AreEqual("Updated City", result.City);
    }

    [Test]
    public void UpdateAddress_ShouldThrowKeyNotFoundException_WhenAddressDoesNotExist()
    {
        // Arrange
        var addressDto = new AddressDto { AddressId = 1, City = "Non-Existent City", State = "", Country = "" };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Address.Address>(addressDto))
            .Throws(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressService.UpdateAddress(addressDto));
    }

    [Test]
    public async Task DeleteAddressById_ShouldReturnDeletedAddressDto_WhenAddressExists()
    {
        // Arrange
        var addressId = 1;
        var addressEntity = new MatrimonyApiService.Address.Address
            { Id = addressId, City = "City to Delete", State = "", Country = "" };
        var addressDto = new AddressDto { AddressId = addressId, City = "City to Delete", State = "", Country = "" };

        _mockRepo.Setup(repo => repo.DeleteById(addressId)).ReturnsAsync(addressEntity);
        _mockMapper.Setup(mapper => mapper.Map<AddressDto>(addressEntity)).Returns(addressDto);

        // Act
        var result = await _addressService.DeleteAddressById(addressId);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(addressId, result.AddressId);
        ClassicAssert.AreEqual("City to Delete", result.City);
    }

    [Test]
    public void DeleteAddressById_ShouldThrowException_WhenErrorOccurs()
    {
        // Arrange
        var addressId = 1;

        _mockRepo.Setup(repo => repo.DeleteById(addressId)).Throws(new KeyNotFoundException("Error Result"));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _addressService.DeleteAddressById(addressId));
        ClassicAssert.IsTrue(ex.Message.Contains("Error Result"));
    }
}