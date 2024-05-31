using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.MatchRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.MatchRequest;

public class MatchRequestControllerTests
{
    private Mock<IMatchRequestService> _matchRequestServiceMock;
    private Mock<ILogger<MatchRequestController>> _loggerMock;
    private MatchRequestController _matchRequestController;

    [SetUp]
    public void SetUp()
    {
        _matchRequestServiceMock = new Mock<IMatchRequestService>();
        _loggerMock = new Mock<ILogger<MatchRequestController>>();
        _matchRequestController = new MatchRequestController(_matchRequestServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Get_ReturnsOk_WhenMatchExists()
    {
        // Arrange
        var matchId = 1;
        var matchDto = new MatchRequestDto { MatchId = matchId };
        _matchRequestServiceMock.Setup(service => service.GetById(matchId)).ReturnsAsync(matchDto);

        // Act
        var result = await _matchRequestController.Get(matchId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matchDto, result.Value);
    }

    [Test]
    public async Task Get_ReturnsNotFound_WhenMatchDoesNotExist()
    {
        // Arrange
        var matchId = 1;
        _matchRequestServiceMock.Setup(service => service.GetById(matchId)).ThrowsAsync(new KeyNotFoundException("Match not found"));

        // Act
        var result = await _matchRequestController.Get(matchId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetAcceptedMatches_ReturnsOk_WhenMatchesExist()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatchRequestDto> { new MatchRequestDto { MatchId = 1 } };
        _matchRequestServiceMock.Setup(service => service.GetAcceptedMatcheRequests(profileId)).ReturnsAsync(matches);

        // Act
        var result = await _matchRequestController.GetAcceptedMatches(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matches, result.Value);
    }

    [Test]
    public async Task GetMatchRequests_ReturnsOk_WhenMatchesExist()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatchRequestDto> { new MatchRequestDto { MatchId = 1 } };
        _matchRequestServiceMock.Setup(service => service.GetMatchRequests(profileId)).ReturnsAsync(matches);

        // Act
        var result = await _matchRequestController.GetMatchRequests(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matches, result.Value);
    }

    [Test]
    public async Task GetSentMatchRequests_ReturnsOk_WhenMatchesExist()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatchRequestDto> { new MatchRequestDto { MatchId = 1 } };
        _matchRequestServiceMock.Setup(service => service.GetSentMatchRequests(profileId)).ReturnsAsync(matches);

        // Act
        var result = await _matchRequestController.GetSentMatchRequests(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matches, result.Value);
    }

    [Test]
    public async Task GetSentMatchRequests_ReturnsNotFound_WhenMatchesDoNotExist()
    {
        // Arrange
        var profileId = 1;
        _matchRequestServiceMock.Setup(service => service.GetSentMatchRequests(profileId)).ThrowsAsync(new KeyNotFoundException("Matches not found"));

        // Act
        var result = await _matchRequestController.GetSentMatchRequests(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task Reject_ReturnsOk_WhenRequestIsRejected()
    {
        // Arrange
        var matchId = 1;
        var profileId = 1;
        _matchRequestServiceMock.Setup(service => service.Reject(matchId, profileId)).Returns(Task.CompletedTask);

        // Act
        var result = await _matchRequestController.Reject(matchId, profileId) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task Reject_ReturnsBadRequest_WhenInvalidMatchForProfile()
    {
        // Arrange
        var matchId = 1;
        var profileId = 1;
        _matchRequestServiceMock.Setup(service => service.Reject(matchId, profileId)).ThrowsAsync(new InvalidMatchForProfile("Invalid match"));

        // Act
        var result = await _matchRequestController.Reject(matchId, profileId) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task Approve_ReturnsOk_WhenRequestIsApproved()
    {
        // Arrange
        var matchId = 1;
        var profileId = 1;
        _matchRequestServiceMock.Setup(service => service.Approve(matchId, profileId)).Returns(Task.CompletedTask);

        // Act
        var result = await _matchRequestController.Approve(matchId, profileId) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task Approve_ReturnsBadRequest_WhenInvalidMatchForProfile()
    {
        // Arrange
        var matchId = 1;
        var profileId = 1;
        _matchRequestServiceMock.Setup(service => service.Approve(matchId, profileId)).ThrowsAsync(new InvalidMatchForProfile("Invalid match"));

        // Act
        var result = await _matchRequestController.Approve(matchId, profileId) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task MatchRequestToProfile_ReturnsOk_WhenRequestIsSuccessful()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        var matchDto = new MatchRequestDto { MatchId = 1 };
        _matchRequestServiceMock.Setup(service => service.MatchRequestToProfile(senderId, targetId)).ReturnsAsync(matchDto);

        // Act
        var result = await _matchRequestController.MatchRequestToProfile(senderId, targetId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matchDto, result.Value);
    }

    [Test]
    public async Task MatchRequestToProfile_ReturnsBadRequest_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        var errorMessage = "Database error";
        _matchRequestServiceMock.Setup(service => service.MatchRequestToProfile(senderId, targetId)).ThrowsAsync(new DbUpdateException(errorMessage));

        // Act
        var result = await _matchRequestController.MatchRequestToProfile(senderId, targetId) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        ClassicAssert.AreEqual(errorMessage, ((ErrorModel)result.Value).Message);
    }

    [Test]
    public async Task MatchRequestToProfile_ReturnsNotFound_WhenKeyNotFoundExceptionOccurs()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        _matchRequestServiceMock.Setup(service => service.MatchRequestToProfile(senderId, targetId)).ThrowsAsync(new KeyNotFoundException("Match not found"));

        // Act
        var result = await _matchRequestController.MatchRequestToProfile(senderId, targetId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task MatchRequestToProfile_ReturnsNotFound_WhenDuplicateRequestException()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        _matchRequestServiceMock.Setup(service => service.MatchRequestToProfile(senderId, targetId)).ThrowsAsync(new DuplicateRequestException("msg"));

        // Act
        var result = await _matchRequestController.MatchRequestToProfile(senderId, targetId) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    
    [Test]
    public async Task MatchRequestToProfile_ReturnsNotFound_MatchRequestToSelfException()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        _matchRequestServiceMock.Setup(service => service.MatchRequestToProfile(senderId, targetId)).ThrowsAsync(new MatchRequestToSelfException("msg"));

        // Act
        var result = await _matchRequestController.MatchRequestToProfile(senderId, targetId) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }
    [Test]
    public async Task DeleteById_ReturnsOk_WhenMatchIsDeleted()
    {
        // Arrange
        var matchId = 1;
        var matchDto = new MatchRequestDto { MatchId = matchId };
        _matchRequestServiceMock.Setup(service => service.DeleteById(matchId)).ReturnsAsync(matchDto);

        // Act
        var result = await _matchRequestController.DeleteById(matchId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matchDto, result.Value);
    }

    [Test]
    public async Task DeleteById_ReturnsNotFound_WhenMatchDoesNotExist()
    {
        // Arrange
        var matchId = 1;
        _matchRequestServiceMock.Setup(service => service.DeleteById(matchId)).ThrowsAsync(new KeyNotFoundException("Match not found"));

        // Act
        var result = await _matchRequestController.DeleteById(matchId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WhenMatchesExist()
    {
        // Arrange
        var matches = new List<MatchRequestDto> { new MatchRequestDto { MatchId = 1 } };
        _matchRequestServiceMock.Setup(service => service.GetAll()).ReturnsAsync(matches);

        // Act
        var result = await _matchRequestController.GetAll() as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matches, result.Value);
    }
}