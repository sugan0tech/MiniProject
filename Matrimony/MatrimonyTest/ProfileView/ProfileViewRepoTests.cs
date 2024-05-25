using MatrimonyApiService.Commons;
using MatrimonyApiService.ProfileView;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.ProfileView;

[TestFixture]
public class ProfileViewRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private ProfileViewRepo _profileViewRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase(databaseName: "MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _profileViewRepo = new ProfileViewRepo(_context);
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
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };
        await _context.ProfileViews.AddAsync(profileView);
        await _context.SaveChangesAsync();

        // Act
        var result = await _profileViewRepo.GetById(profileView.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.ViewerId);
        ClassicAssert.AreEqual(2, result.ViewedProfileAt);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileViewRepo.GetById(99));
        ClassicAssert.AreEqual("ProfileView with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.ProfileViews.AddRangeAsync(
            new MatrimonyApiService.ProfileView.ProfileView
            {
                ViewerId = 1,
                ViewedProfileAt = 2,
                ViewedAt = DateTime.Now
            },
            new MatrimonyApiService.ProfileView.ProfileView
            {
                ViewerId = 3,
                ViewedProfileAt = 4,
                ViewedAt = DateTime.Now
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _profileViewRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };

        // Act
        var result = await _profileViewRepo.Add(profileView);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(1, await _context.ProfileViews.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _profileViewRepo.Add(null));
        ClassicAssert.AreEqual("ProfileView cannot be null. (Parameter 'entity')", ex.Message);
    }
}