using MatrimonyApiService.Commons;
using MatrimonyApiService.Message;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Message;

[TestFixture]
public class MessageRepoTest
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private MessageRepo _messageRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase("MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _messageRepo = new MessageRepo(_context);
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
        var message = new MatrimonyApiService.Message.Message
        {
            SenderId = 1,
            ReceiverId = 2,
            SentAt = DateTime.Now,
            Seen = false
        };
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        // Act
        var result = await _messageRepo.GetById(message.Id);

        // Assert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.SenderId);
        ClassicAssert.AreEqual(2, result.ReceiverId);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageRepo.GetById(99));
        ClassicAssert.AreEqual("Message with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Messages.AddRangeAsync(
            new MatrimonyApiService.Message.Message
                { SenderId = 1, ReceiverId = 2, SentAt = DateTime.Now, Seen = false },
            new MatrimonyApiService.Message.Message
                { SenderId = 2, ReceiverId = 1, SentAt = DateTime.Now, Seen = true }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _messageRepo.GetAll();

        // Assert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var message = new MatrimonyApiService.Message.Message
        {
            SenderId = 1,
            ReceiverId = 2,
            SentAt = DateTime.Now,
            Seen = false
        };

        // Act
        var result = await _messageRepo.Add(message);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(1, result.SenderId);
        ClassicAssert.AreEqual(1, await _context.Messages.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _messageRepo.Add(null));
        ClassicAssert.AreEqual("Message cannot be null. (Parameter 'entity')", ex.Message);
    }

    [Test]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var message = new MatrimonyApiService.Message.Message
        {
            SenderId = 1,
            ReceiverId = 2,
            SentAt = DateTime.Now,
            Seen = false
        };
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        message.Seen = true;

        // Act
        var result = await _messageRepo.Update(message);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(true, result.Seen);
    }

    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Arrange
        var updateMessage = new MatrimonyApiService.Message.Message
        {
            Id = 99,
            SenderId = 1,
            ReceiverId = 2,
            SentAt = DateTime.Now,
            Seen = false
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageRepo.Update(updateMessage));
        ClassicAssert.AreEqual("Message with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task DeleteById_ShouldDeleteEntity()
    {
        // Arrange
        var message = new MatrimonyApiService.Message.Message
        {
            SenderId = 1,
            ReceiverId = 2,
            SentAt = DateTime.Now,
            Seen = false
        };
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        // Act
        await _messageRepo.DeleteById(message.Id);

        // Assert
        ClassicAssert.AreEqual(0, await _context.Messages.CountAsync());
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageRepo.DeleteById(99));
        ClassicAssert.AreEqual("Message with key 99 not found!!!", ex.Message);
    }
}