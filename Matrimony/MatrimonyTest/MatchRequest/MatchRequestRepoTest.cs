using MatrimonyApiService.Commons;
using MatrimonyApiService.MatchRequest;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.MatchRequest;

[TestFixture]
public class MatchRequestRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private MatchRequestRepo _matchRequestRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase("MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _matchRequestRepo = new MatchRequestRepo(_context);
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
        var match = new MatrimonyApiService.MatchRequest.MatchRequest
        {
            SentProfileId = 1, ReceivedProfileId = 2, ProfileOneLike = true, ProfileTwoLike = false, Level = 3,
            FoundAt = DateTime.Now
        };
        await _context.Matches.AddAsync(match);
        await _context.SaveChangesAsync();

        // Act
        var result = await _matchRequestRepo.GetById(match.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.SentProfileId);
        ClassicAssert.AreEqual(2, result.ReceivedProfileId);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _matchRequestRepo.GetById(99));
        ClassicAssert.AreEqual("MatchRequest with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Matches.AddRangeAsync(
            new MatrimonyApiService.MatchRequest.MatchRequest
            {
                SentProfileId = 1, ReceivedProfileId = 2, ProfileOneLike = true, ProfileTwoLike = false, Level = 3,
                FoundAt = DateTime.Now
            },
            new MatrimonyApiService.MatchRequest.MatchRequest
            {
                SentProfileId = 3, ReceivedProfileId = 4, ProfileOneLike = false, ProfileTwoLike = true, Level = 4,
                FoundAt = DateTime.Now
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _matchRequestRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var match = new MatrimonyApiService.MatchRequest.MatchRequest
        {
            SentProfileId = 1, ReceivedProfileId = 2, ProfileOneLike = true, ProfileTwoLike = false, Level = 3,
            FoundAt = DateTime.Now
        };

        // Act
        var result = await _matchRequestRepo.Add(match);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(1, result.SentProfileId);
        ClassicAssert.AreEqual(1, await _context.Matches.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _matchRequestRepo.Add(null));
        ClassicAssert.AreEqual("MatchRequest cannot be null. (Parameter 'entity')", ex.Message);
    }

    [Test]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var match = new MatrimonyApiService.MatchRequest.MatchRequest
        {
            SentProfileId = 1, ReceivedProfileId = 2, ProfileOneLike = true, ProfileTwoLike = false, Level = 3,
            FoundAt = DateTime.Now
        };
        await _context.Matches.AddAsync(match);
        await _context.SaveChangesAsync();

        match.ProfileTwoLike = true;

        // Act
        var result = await _matchRequestRepo.Update(match);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(true, result.ProfileTwoLike);
    }

    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Arrange
        var updateMatch = new MatrimonyApiService.MatchRequest.MatchRequest
        {
            Id = 99, SentProfileId = 1, ReceivedProfileId = 2, ProfileOneLike = true, ProfileTwoLike = false,
            Level = 3, FoundAt = DateTime.Now
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _matchRequestRepo.Update(updateMatch));
        ClassicAssert.AreEqual("MatchRequest with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task DeleteById_ShouldDeleteEntity()
    {
        // Arrange
        var match = new MatrimonyApiService.MatchRequest.MatchRequest
        {
            SentProfileId = 1, ReceivedProfileId = 2, ProfileOneLike = true, ProfileTwoLike = false, Level = 3,
            FoundAt = DateTime.Now
        };
        await _context.Matches.AddAsync(match);
        await _context.SaveChangesAsync();

        // Act
        await _matchRequestRepo.DeleteById(match.Id);

        // Assert
        ClassicAssert.AreEqual(0, await _context.Matches.CountAsync());
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _matchRequestRepo.DeleteById(99));
        ClassicAssert.AreEqual("MatchRequest with key 99 not found!!!", ex.Message);
    }
}