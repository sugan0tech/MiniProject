using MatrimonyApiService.Commons;
using MatrimonyApiService.User;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.User;

[TestFixture]
public class UserRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private UserRepo _userRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _userRepo = new UserRepo(_context);
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
        var user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepo.GetById(user.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("user@example.com", result.Email);
        ClassicAssert.AreEqual("John", result.FirstName);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _userRepo.GetById(99));
        ClassicAssert.AreEqual("User with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Users.AddRangeAsync(
            new MatrimonyApiService.User.User
            {
                Email = "user1@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                IsVerified = true
            },
            new MatrimonyApiService.User.User
            {
                Email = "user2@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "9876543210",
                IsVerified = true
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _userRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true
        };

        // Act
        var result = await _userRepo.Add(user);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual("user@example.com", result.Email);
        ClassicAssert.AreEqual(1, await _context.Users.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepo.Add(null));
        ClassicAssert.AreEqual("User cannot be null. (Parameter 'entity')", ex.Message);
    }
}