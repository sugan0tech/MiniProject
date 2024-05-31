using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Message;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Message;

public class MessageServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Message.Message>> _mockRepo;
    private Mock<IMembershipService> _mockMembershipService;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<MessageService>> _mockLogger;
    private MessageService _messageService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.Message.Message>>();
        _mockMapper = new Mock<IMapper>();
        _mockMembershipService = new Mock<IMembershipService>();
        _mockLogger = new Mock<ILogger<MessageService>>();
        _messageService = new MessageService(_mockRepo.Object, _mockMembershipService.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task AddMessage_ValidMessageDto_ReturnsAddedMessageDto()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };
        var message = new MatrimonyApiService.Message.Message { Id = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Message.Message>(messageDto)).Returns(message);
        _mockRepo.Setup(repo => repo.Add(message)).ReturnsAsync(message);
        _mockMapper.Setup(mapper => mapper.Map<MessageDto>(message)).Returns(messageDto);

        // Act
        var result = await _messageService.AddMessage(messageDto);

        // ClassicAssert
        ClassicAssert.AreEqual(messageDto, result);
    }

    [Test]
    public void AddMessage_ThrowsException_ReturnsException()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Message.Message>(messageDto))
            .Throws(new Exception("Error adding message"));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _messageService.AddMessage(messageDto));
        ClassicAssert.AreEqual("Error adding message", ex.Message);
    }

    [Test]
    public async Task GetMessageById_ValidId_ReturnsMessageDto()
    {
        // Arrange
        var messageId = 1;
        var message = new MatrimonyApiService.Message.Message { Id = messageId, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };
        var messageDto = new MessageDto { MessageId = messageId, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };

        _mockRepo.Setup(repo => repo.GetById(messageId)).ReturnsAsync(message);
        _mockMapper.Setup(mapper => mapper.Map<MessageDto>(message)).Returns(messageDto);

        // Act
        var result = await _messageService.GetMessageById(messageId);

        // ClassicAssert
        ClassicAssert.AreEqual(messageDto, result);
    }

    [Test]
    public void GetMessageById_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var messageId = 1;
        _mockRepo.Setup(repo => repo.GetById(messageId)).Throws(new KeyNotFoundException("Message not found"));

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageService.GetMessageById(messageId));
    }

    [Test]
    public async Task UpdateMessage_ValidMessageDto_ReturnsUpdatedMessageDto()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };
        var message = new MatrimonyApiService.Message.Message { Id = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };

        _mockRepo.Setup(repo => repo.GetById(messageDto.MessageId)).ReturnsAsync(message);
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Message.Message>(messageDto)).Returns(message);
        _mockRepo.Setup(repo => repo.Update(message)).ReturnsAsync(message);
        _mockMapper.Setup(mapper => mapper.Map<MessageDto>(message)).Returns(messageDto);

        // Act
        var result = await _messageService.UpdateMessage(messageDto);

        // ClassicAssert
        ClassicAssert.AreEqual(messageDto, result);
    }

    [Test]
    public void UpdateMessage_InvalidMessageDto_ThrowsKeyNotFoundException()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };

        var message = new MatrimonyApiService.Message.Message { Id = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Message.Message>(It.IsAny<MessageDto>())).Returns(message);
        _mockRepo.Setup(repo => repo.Update(message)).Throws(new KeyNotFoundException("Message not found"));

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageService.UpdateMessage(messageDto));
    }

    [Test]
    public async Task DeleteMessageById_ValidId_ReturnsDeletedMessageDto()
    {
        // Arrange
        var messageId = 1;
        var message = new MatrimonyApiService.Message.Message { Id = messageId, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };
        var messageDto = new MessageDto { MessageId = messageId, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false };

        _mockRepo.Setup(repo => repo.DeleteById(messageId)).ReturnsAsync(message);
        _mockMapper.Setup(mapper => mapper.Map<MessageDto>(message)).Returns(messageDto);

        // Act
        var result = await _messageService.DeleteMessageById(messageId);

        // ClassicAssert
        ClassicAssert.AreEqual(messageDto, result);
    }

    [Test]
    public void DeleteMessageById_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var messageId = 1;
        _mockRepo.Setup(repo => repo.DeleteById(messageId)).Throws(new KeyNotFoundException("Message not found"));

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageService.DeleteMessageById(messageId));
    }

    [Test]
    public async Task GetAllMessages_ReturnsListOfMessageDto()
    {
        // Arrange
        var messages = new List<MatrimonyApiService.Message.Message>
        {
            new MatrimonyApiService.Message.Message { Id = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false },
            new MatrimonyApiService.Message.Message { Id = 2, SenderId = 2, ReceiverId = 1, SentAt = DateTime.UtcNow, Seen = true }
        };
        var messageDtos = new List<MessageDto>
        {
            new MessageDto { MessageId = 1, SenderId = 1, ReceiverId = 2, SentAt = DateTime.UtcNow, Seen = false },
            new MessageDto { MessageId = 2, SenderId = 2, ReceiverId = 1, SentAt = DateTime.UtcNow, Seen = true }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(messages);
        _mockMapper.Setup(mapper => mapper.Map<List<MessageDto>>(messages)).Returns(messageDtos);

        // Act
        var result = await _messageService.GetAllMessages();

        // ClassicAssert
        ClassicAssert.AreEqual(messageDtos, result);
    }

    [Test]
    public async Task GetSentMessages_ValidUserId_ReturnsListOfMessageDto()
    {
        // Arrange
        var userId = 1;
        var messages = new List<MatrimonyApiService.Message.Message>
        {
            new MatrimonyApiService.Message.Message { Id = 1, SenderId = userId, ReceiverId = 2, SentAt = DateTime.Now, Seen = false },
            new MatrimonyApiService.Message.Message { Id = 2, SenderId = userId, ReceiverId = 3, SentAt = DateTime.Now, Seen = true }
        };
        var messageDtos = new List<MessageDto>
        {
            new MessageDto { MessageId = 1, SenderId = userId, ReceiverId = 2, SentAt = DateTime.Now, Seen = false },
            new MessageDto { MessageId = 2, SenderId = userId, ReceiverId = 3, SentAt = DateTime.Now, Seen = true }
        };

        var membershipDto = new MembershipDto { MembershipId = 1, ProfileId = 1, Type = MemberShip.PremiumUser.ToString(), Description = "Test Description" };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(messages);
        _mockMembershipService.Setup(service => service.GetByUserId(userId)).ReturnsAsync(membershipDto);
        _mockMapper.Setup(mapper => mapper.Map<MessageDto>(It.IsAny<MatrimonyApiService.Message.Message>()))
            .Returns((MatrimonyApiService.Message.Message src) => messageDtos.Find(dto => dto.MessageId == src.Id));

        // Act
        var result = await _messageService.GetSentMessages(userId);

        // ClassicAssert
        ClassicAssert.AreEqual(messageDtos.Count, result.Count);
    }

    [Test]
    public void GetSentMessages_NonPremiumUser_ThrowsNonPremiumUserException()
    {
        // Arrange
        var userId = 1;
        var membershipDto = new MembershipDto { MembershipId = 1, ProfileId = 1, Type = MemberShip.BasicUser.ToString(), Description = "Test Description" };

        _mockMembershipService.Setup(service => service.GetByUserId(userId)).ReturnsAsync(membershipDto);

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<NonPremiumUserException>(async () => await _messageService.GetSentMessages(userId));
        ClassicAssert.AreEqual("You should be a Premium user for accessing chats", ex.Message);
    }

    [Test]
    public void GetSentMessages_InvalidUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var userId = 1;
        _mockMembershipService.Setup(service => service.GetByUserId(userId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageService.GetSentMessages(userId));
    }

    [Test]
    public async Task GetReceivedMessages_ValidUserId_ReturnsListOfMessageDto()
    {
        // Arrange
        var userId = 1;
        var messages = new List<MatrimonyApiService.Message.Message>
        {
            new MatrimonyApiService.Message.Message { Id = 1, SenderId = 2, ReceiverId = userId, SentAt = DateTime.Now, Seen = false },
            new MatrimonyApiService.Message.Message { Id = 2, SenderId = 3, ReceiverId = userId, SentAt = DateTime.Now, Seen = true }
        };
        var messageDtos = new List<MessageDto>
        {
            new MessageDto { MessageId = 1, SenderId = 2, ReceiverId = userId, SentAt = DateTime.Now, Seen = false },
            new MessageDto { MessageId = 2, SenderId = 3, ReceiverId = userId, SentAt = DateTime.Now, Seen = true }
        };

        var membershipDto = new MembershipDto { MembershipId = 1, ProfileId = 1, Type = MemberShip.PremiumUser.ToString(), Description = "Test Description" };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(messages);
        _mockMembershipService.Setup(service => service.GetByUserId(userId)).ReturnsAsync(membershipDto);
        _mockMapper.Setup(mapper => mapper.Map<MessageDto>(It.IsAny<MatrimonyApiService.Message.Message>()))
            .Returns((MatrimonyApiService.Message.Message src) => messageDtos.Find(dto => dto.MessageId == src.Id));

        // Act
        var result = await _messageService.GetReceivedMessages(userId);

        // ClassicAssert
        ClassicAssert.AreEqual(messageDtos.Count, result.Count);
    }

    [Test]
    public void GetReceivedMessages_NonPremiumUser_ThrowsNonPremiumUserException()
    {
        // Arrange
        var userId = 1;
        var membershipDto = new MembershipDto { MembershipId = 1, ProfileId = 1, Type = MemberShip.BasicUser.ToString(), Description = "Test Description" };

        _mockMembershipService.Setup(service => service.GetByUserId(userId)).ReturnsAsync(membershipDto);

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<NonPremiumUserException>(async () => await _messageService.GetReceivedMessages(userId));
        ClassicAssert.AreEqual("You should be a Premium user for accessing chats", ex.Message);
    }

    [Test]
    public void GetReceived_InvalidUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var userId = 1;
        _mockMembershipService.Setup(service => service.GetByUserId(userId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _messageService.GetReceivedMessages(userId));
    }
}