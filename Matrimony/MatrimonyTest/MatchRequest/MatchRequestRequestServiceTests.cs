using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Profile;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.MatchRequest;

public class MatchRequestRequestServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.MatchRequest.MatchRequest>> _mockRepo;
    private Mock<IProfileService> _mockProfileService;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<MatchRequestRequestService>> _mockLogger;
    private MatchRequestRequestService _matchRequestRequestService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.MatchRequest.MatchRequest>>();
        _mockProfileService = new Mock<IProfileService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MatchRequestRequestService>>();
        _matchRequestRequestService = new MatchRequestRequestService(_mockRepo.Object, _mockProfileService.Object,
            _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAcceptedMatches_ValidProfileId_ReturnsAcceptedMatches()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatrimonyApiService.MatchRequest.MatchRequest>
        {
            new() { Id = 1, SentProfileId = profileId, ProfileTwoLike = true },
            new() { Id = 2, SentProfileId = profileId, ProfileTwoLike = false },
            new() { Id = 3, SentProfileId = 2, ProfileTwoLike = true }
        };
        var expectedMatches = new List<MatchRequestDto>
        {
            new() { MatchId = 1, SentProfileId = profileId, ProfileTwoLike = true }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(matches);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>()))
            .Returns((MatrimonyApiService.MatchRequest.MatchRequest source) => new MatchRequestDto
                { MatchId = source.Id, SentProfileId = source.SentProfileId, ProfileTwoLike = source.ProfileTwoLike });

        // Act
        var result = await _matchRequestRequestService.GetAcceptedMatches(profileId);

        // ClassicAssert
        ClassicAssert.AreEqual(1, result.Count);
        ClassicAssert.AreEqual(expectedMatches[0].MatchId, result[0].MatchId);
        ClassicAssert.AreEqual(expectedMatches[0].SentProfileId, result[0].SentProfileId);
        ClassicAssert.AreEqual(expectedMatches[0].ProfileTwoLike, result[0].ProfileTwoLike);
    }

    [Test]
    public async Task GetMatchRequests_ValidProfileId_ReturnsMatchRequests()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatrimonyApiService.MatchRequest.MatchRequest>
        {
            new() { Id = 1, ReceivedProfileId = profileId },
            new() { Id = 2, ReceivedProfileId = 2 },
            new() { Id = 3, ReceivedProfileId = profileId }
        };
        var expectedMatches = new List<MatchRequestDto>
        {
            new() { MatchId = 1, ReceivedProfileId = profileId },
            new() { MatchId = 3, ReceivedProfileId = profileId }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(matches);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>()))
            .Returns((MatrimonyApiService.MatchRequest.MatchRequest source) => new MatchRequestDto
                { MatchId = source.Id, ReceivedProfileId = source.ReceivedProfileId });

        // Act
        var result = await _matchRequestRequestService.GetMatchRequests(profileId);

        // ClassicAssert
        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.AreEqual(expectedMatches[0].MatchId, result[0].MatchId);
        ClassicAssert.AreEqual(expectedMatches[0].ReceivedProfileId, result[0].ReceivedProfileId);
        ClassicAssert.AreEqual(expectedMatches[1].MatchId, result[1].MatchId);
        ClassicAssert.AreEqual(expectedMatches[1].ReceivedProfileId, result[1].ReceivedProfileId);
    }

    [Test]
    public async Task Cancel_ValidMatchIdAndProfileId_UpdatesMatch()
    {
        // Arrange
        var matchId = 1;
        var profileId = 2;
        var match = new MatrimonyApiService.MatchRequest.MatchRequest
            { Id = matchId, ReceivedProfileId = profileId, ProfileTwoLike = true };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);

        // Act
        await _matchRequestRequestService.Cancel(matchId, profileId);

        // ClassicAssert
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.MatchRequest.MatchRequest>(m => m.Id == matchId && m.ProfileTwoLike == false)), Times.Once);
    }

    [Test]
    public void Cancel_InvalidMatchId_ThrowsInvalidMatchForProfile()
    {
        // Arrange
        var matchId = 1;
        var profileId = 3;
        var match = new MatrimonyApiService.MatchRequest.MatchRequest { Id = matchId, ReceivedProfileId = 2 };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<InvalidMatchForProfile>(
            () => _matchRequestRequestService.Cancel(matchId, profileId));
        ClassicAssert.AreEqual($"The match {matchId} is not meant for {profileId}", ex.Message);
    }

    [Test]
    public async Task GetById_ValidId_ReturnsMatchDto()
    {
        // Arrange
        var matchId = 1;
        var match = new MatrimonyApiService.MatchRequest.MatchRequest { Id = matchId, SentProfileId = 1 };
        var expectedMatchDto = new MatchRequestDto { MatchId = matchId, SentProfileId = 1 };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(match)).Returns(expectedMatchDto);

        // Act
        var result = await _matchRequestRequestService.GetById(matchId);

        // ClassicAssert
        ClassicAssert.AreEqual(expectedMatchDto, result);
    }

    [Test]
    public void GetById_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var matchId = 1;

        _mockRepo.Setup(repo => repo.GetById(matchId)).Throws(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _matchRequestRequestService.GetById(matchId));
    }

    [Test]
    public async Task Add_ValidDto_ReturnsAddedMatchDto()
    {
        // Arrange
        var matchDto = new MatchRequestDto { MatchId = 1, SentProfileId = 1 };
        var match = new MatrimonyApiService.MatchRequest.MatchRequest { Id = 1, SentProfileId = 1 };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.MatchRequest.MatchRequest>(matchDto)).Returns(match);
        _mockRepo.Setup(repo => repo.Add(match)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(match)).Returns(matchDto);

        // Act
        var result = await _matchRequestRequestService.Add(matchDto);

        // ClassicAssert
        ClassicAssert.AreEqual(matchDto, result);
    }

    [Test]
    public async Task Update_ValidDto_ReturnsUpdatedMatchDto()
    {
        // Arrange
        var matchDto = new MatchRequestDto { MatchId = 1, SentProfileId = 1 };
        var match = new MatrimonyApiService.MatchRequest.MatchRequest { Id = 1, SentProfileId = 1 };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.MatchRequest.MatchRequest>(matchDto)).Returns(match);
        _mockRepo.Setup(repo => repo.Update(match)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(match)).Returns(matchDto);

        // Act
        var result = await _matchRequestRequestService.Update(matchDto);

        // ClassicAssert
        ClassicAssert.AreEqual(matchDto, result);
    }

    [Test]
    public async Task DeleteById_ValidId_ReturnsDeletedMatchDto()
    {
        // Arrange
        var matchId = 1;
        var match = new MatrimonyApiService.MatchRequest.MatchRequest { Id = matchId, SentProfileId = 1 };
        var expectedMatchDto = new MatchRequestDto { MatchId = matchId, SentProfileId = 1 };

        _mockRepo.Setup(repo => repo.DeleteById(matchId)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(match)).Returns(expectedMatchDto);

        // Act
        var result = await _matchRequestRequestService.DeleteById(matchId);

        // ClassicAssert
        ClassicAssert.AreEqual(expectedMatchDto, result);
    }
}