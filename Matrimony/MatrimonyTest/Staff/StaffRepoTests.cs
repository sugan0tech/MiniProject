using MatrimonyApiService.Commons;
using MatrimonyApiService.Staff;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Staff;

[TestFixture]
public class StaffRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private StaffRepo _staffRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _staffRepo = new StaffRepo(_context);
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
        var staff = new MatrimonyApiService.Staff.Staff
        {
            Email = "staff@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Role = "Admin",
            IsVerified = true
        };
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();

        // Act
        var result = await _staffRepo.GetById(staff.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("staff@example.com", result.Email);
        ClassicAssert.AreEqual("John", result.FirstName);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _staffRepo.GetById(99));
        ClassicAssert.AreEqual("Staff with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Staffs.AddRangeAsync(
            new MatrimonyApiService.Staff.Staff
            {
                Email = "staff1@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Role = "Admin",
                IsVerified = true
            },
            new MatrimonyApiService.Staff.Staff
            {
                Email = "staff2@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "9876543210",
                Role = "Manager",
                IsVerified = true
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _staffRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var staff = new MatrimonyApiService.Staff.Staff
        {
            Email = "staff@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Role = "Admin",
            IsVerified = true
        };

        // Act
        var result = await _staffRepo.Add(staff);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("staff@example.com", result.Email);
        ClassicAssert.AreEqual(1, await _context.Staffs.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _staffRepo.Add(null));
        ClassicAssert.AreEqual("Staff cannot be null. (Parameter 'entity')", ex.Message);
    }
}