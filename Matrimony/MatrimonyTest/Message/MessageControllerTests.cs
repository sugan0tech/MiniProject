using System.Security.Claims;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Message;

public class MessageControllerTests
{
    private Mock<IMessageService> _messageServiceMock;
    private Mock<ILogger<MessageController>> _loggerMock;
    private MessageController _messageController;
    private List<Claim> _claims;

    [SetUp]
    public void SetUp()
    {
        _messageServiceMock = new Mock<IMessageService>();
        _loggerMock = new Mock<ILogger<MessageController>>();
        _messageController = new MessageController(_messageServiceMock.Object, _loggerMock.Object);

        _claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };
        var identity = new ClaimsIdentity(_claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _messageController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Test]
    public async Task AddMessage_ReturnsOk_WhenMessageIsAdded()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 1};
        _messageServiceMock.Setup(service => service.AddMessage(messageDto)).ReturnsAsync(messageDto);

        // Act
        var result = await _messageController.AddMessage(messageDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(messageDto, result.Value);
    }

    [Test]
    public async Task AddMessage_ReturnsBadRequest_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 1, ReceiverId = 1};
        var dbUpdateException =
            new DbUpdateException("Database update error", new Exception("Inner exception message"));
        _messageServiceMock.Setup(service => service.AddMessage(messageDto)).ThrowsAsync(dbUpdateException);

        // Act
        var result = await _messageController.AddMessage(messageDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task AddMessage_ReturnsForBidden_WhenAuthenticationExceptionOccurs()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 2, ReceiverId = 3};

        // Act
        var result = await _messageController.AddMessage(messageDto) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetMessageById_ReturnsOk_WhenMessageExists()
    {
        // Arrange
        var messageId = 1;
        var messageDto = new MessageDto { MessageId = messageId , SenderId = 1};
        _messageServiceMock.Setup(service => service.GetMessageById(messageId)).ReturnsAsync(messageDto);

        // Act
        var result = await _messageController.GetMessageById(messageId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(messageDto, result.Value);
    }

    [Test]
    public async Task GetMessageById_ReturnsNotFound_WhenMessageDoesNotExist()
    {
        // Arrange
        var messageId = 1;
        _messageServiceMock.Setup(service => service.GetMessageById(messageId))
            .ThrowsAsync(new KeyNotFoundException("Message not found"));

        // Act
        var result = await _messageController.GetMessageById(messageId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetMessageById_ReturnsForBidden_WhenSenderOrReceiverNotAuthorized()
    {
        // Arrange
        var messageDto = new MessageDto { MessageId = 1, SenderId = 2, ReceiverId = 3};

        // Act
        _messageServiceMock.Setup(service => service.GetMessageById(1)).ReturnsAsync(messageDto);
        var result = await _messageController.GetMessageById(1) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task DeleteMessageById_ReturnsOk_WhenMessageIsDeleted()
    {
        // Arrange
        var messageId = 1;
        var messageDto = new MessageDto { MessageId = messageId };
        _messageServiceMock.Setup(service => service.DeleteMessageById(messageId)).ReturnsAsync(messageDto);

        // Act
        var result = await _messageController.DeleteMessageById(messageId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(messageDto, result.Value);
    }

    [Test]
    public async Task DeleteMessageById_ReturnsNotFound_WhenMessageDoesNotExist()
    {
        // Arrange
        var messageId = 1;
        _messageServiceMock.Setup(service => service.DeleteMessageById(messageId))
            .ThrowsAsync(new KeyNotFoundException("Message not found"));

        // Act
        var result = await _messageController.DeleteMessageById(messageId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetAllMessages_ReturnsOk_WhenMessagesExist()
    {
        // Arrange
        var messages = new List<MessageDto> { new MessageDto { MessageId = 1 } };
        _messageServiceMock.Setup(service => service.GetAllMessages()).ReturnsAsync(messages);

        // Act
        var result = await _messageController.GetAllMessages() as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(messages, result.Value);
    }

    [Test]
    public async Task GetSentMessages_ReturnsOk_WhenMessagesExist()
    {
        // Arrange
        var userId = 1;
        var messages = new List<MessageDto> { new MessageDto { MessageId = 1 } };
        _messageServiceMock.Setup(service => service.GetSentMessages(userId)).ReturnsAsync(messages);

        // Act
        var result = await _messageController.GetSentMessages(userId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(messages, result.Value);
    }

    [Test]
    public async Task GetSentMessages_ReturnsForbidden_WhenUserIsNonPremium()
    {
        // Arrange
        var userId = 1;
        _messageServiceMock.Setup(service => service.GetSentMessages(userId))
            .ThrowsAsync(new NonPremiumUserException("User is not premium"));

        // Act
        var result = await _messageController.GetSentMessages(userId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetSentMessages_ReturnsForbidden_WhenUserIsWrong()
    {
        // Arrange
        var userId = 2;

        // Act
        var result = await _messageController.GetSentMessages(userId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetSentMessages_ReturnsNotFound_WhenMessagesDoNotExist()
    {
        // Arrange
        var userId = 1;
        _messageServiceMock.Setup(service => service.GetSentMessages(userId))
            .ThrowsAsync(new KeyNotFoundException("Messages not found"));

        // Act
        var result = await _messageController.GetSentMessages(userId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetReceivedMessages_ReturnsOk_WhenMessagesExist()
    {
        // Arrange
        var userId = 1;
        var messages = new List<MessageDto> { new MessageDto { MessageId = 1 } };
        _messageServiceMock.Setup(service => service.GetReceivedMessages(userId)).ReturnsAsync(messages);

        // Act
        var result = await _messageController.GetReceivedMessages(userId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(messages, result.Value);
    }

    [Test]
    public async Task GetReceivedMessages_ReturnsForbidden_WhenUserIsNonPremium()
    {
        // Arrange
        var userId = 1;
        _messageServiceMock.Setup(service => service.GetReceivedMessages(userId))
            .ThrowsAsync(new NonPremiumUserException("User is not premium"));

        // Act
        var result = await _messageController.GetReceivedMessages(userId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetReceivedMessages_ReturnsForbidden_WhenUserDontHaveAccess()
    {
        // Arrange
        var userId = 2;

        // Act
        var result = await _messageController.GetReceivedMessages(userId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetReceivedMessages_ReturnsNotFound_WhenMessagesDoNotExist()
    {
        // Arrange
        var userId = 1;
        _messageServiceMock.Setup(service => service.GetReceivedMessages(userId))
            .ThrowsAsync(new KeyNotFoundException("Messages not found"));

        // Act
        var result = await _messageController.GetReceivedMessages(userId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }
}