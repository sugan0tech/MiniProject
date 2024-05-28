using Microsoft.EntityFrameworkCore;
using MatrimonyApiService.Address;
using MatrimonyApiService.Commons;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Address;

[TestFixture]
public class AddressRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private AddressRepo _addressRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase("MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _addressRepo = new AddressRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetById_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var address = new MatrimonyApiService.Address.Address
            { Id = 1, City = "City1", State = "State1", Country = "Country1" };
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();

        // Act
        var result = await _addressRepo.GetById(1);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("City1", result.City);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressRepo.GetById(99));
        ClassicAssert.AreEqual("Address with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Addresses.AddRangeAsync(
            new MatrimonyApiService.Address.Address
                { Id = 1, Street = "street1", City = "City1", State = "State1", Country = "Country1" },
            new MatrimonyApiService.Address.Address
                { Id = 2, Street = "street2", City = "City2", State = "State2", Country = "Country2" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _addressRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var address = new MatrimonyApiService.Address.Address
            { Id = 1, City = "City1", State = "State1", Country = "Country1" };

        // Act
        var result = await _addressRepo.Add(address);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("City1", result.City);
        ClassicAssert.AreEqual(1, await _context.Addresses.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _addressRepo.Add(null));
        ClassicAssert.AreEqual("Address cannot be null. (Parameter 'entity')", ex.Message);
    }

    [Test]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var address = new MatrimonyApiService.Address.Address
            { Id = 1, City = "City1", State = "State1", Country = "Country1" };
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();

        address.City = "NewCity";

        // Act
        var result = await _addressRepo.Update(address);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("NewCity", result.City);
    }

    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Arrange
        var updateAddress = new MatrimonyApiService.Address.Address
            { Id = 99, City = "City99", State = "State99", Country = "Country99" };

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressRepo.Update(updateAddress));
        ClassicAssert.AreEqual("Address with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task DeleteById_ShouldDeleteEntity()
    {
        // Arrange
        var address = new MatrimonyApiService.Address.Address
            { Id = 1, City = "City1", State = "State1", Country = "Country1" };
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();

        // Act
        await _addressRepo.DeleteById(1);

        // Assert
        ClassicAssert.AreEqual(0, await _context.Addresses.CountAsync());
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _addressRepo.DeleteById(99));
        ClassicAssert.AreEqual("Address with key 99 not found!!!", ex.Message);
    }
}