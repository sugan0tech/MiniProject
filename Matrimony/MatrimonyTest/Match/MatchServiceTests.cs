using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Match;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Match;

public class MatchServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Match.Match>> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<MatchService>> _mockLogger;
    private MatchService _matchService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.Match.Match>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MatchService>>();
        _matchService = new MatchService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAcceptedMatches_ValidProfileId_ReturnsAcceptedMatches()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatrimonyApiService.Match.Match>
        {
            new() { Id = 1, SentProfileId = profileId, ProfileTwoLike = true },
            new() { Id = 2, SentProfileId = profileId, ProfileTwoLike = false },
            new() { Id = 3, SentProfileId = 2, ProfileTwoLike = true }
        };
        var expectedMatches = new List<MatchDto>
        {
            new() { MatchId = 1, SentProfileId = profileId, ProfileTwoLike = true }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(matches);
        _mockMapper.Setup(mapper => mapper.Map<MatchDto>(It.IsAny<MatrimonyApiService.Match.Match>()))
            .Returns((MatrimonyApiService.Match.Match source) => new MatchDto
                { MatchId = source.Id, SentProfileId = source.SentProfileId, ProfileTwoLike = source.ProfileTwoLike });

        // Act
        var result = await _matchService.GetAcceptedMatches(profileId);

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
        var matches = new List<MatrimonyApiService.Match.Match>
        {
            new() { Id = 1, ReceivedProfileId = profileId },
            new() { Id = 2, ReceivedProfileId = 2 },
            new() { Id = 3, ReceivedProfileId = profileId }
        };
        var expectedMatches = new List<MatchDto>
        {
            new() { MatchId = 1, ReceivedProfileId = profileId },
            new() { MatchId = 3, ReceivedProfileId = profileId }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(matches);
        _mockMapper.Setup(mapper => mapper.Map<MatchDto>(It.IsAny<MatrimonyApiService.Match.Match>()))
            .Returns((MatrimonyApiService.Match.Match source) => new MatchDto
                { MatchId = source.Id, ReceivedProfileId = source.ReceivedProfileId });

        // Act
        var result = await _matchService.GetMatchRequests(profileId);

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
        var match = new MatrimonyApiService.Match.Match
            { Id = matchId, ReceivedProfileId = profileId, ProfileTwoLike = true };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);

        // Act
        await _matchService.Cancel(matchId, profileId);

        // ClassicAssert
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.Match.Match>(m => m.Id == matchId && m.ProfileTwoLike == false)), Times.Once);
    }

    [Test]
    public void Cancel_InvalidMatchId_ThrowsInvalidMatchForProfile()
    {
        // Arrange
        var matchId = 1;
        var profileId = 3;
        var match = new MatrimonyApiService.Match.Match { Id = matchId, ReceivedProfileId = 2 };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<InvalidMatchForProfile>(() => _matchService.Cancel(matchId, profileId));
        ClassicAssert.AreEqual($"The match {matchId} is not meant for {profileId}", ex.Message);
    }

    [Test]
    public async Task GetById_ValidId_ReturnsMatchDto()
    {
        // Arrange
        var matchId = 1;
        var match = new MatrimonyApiService.Match.Match { Id = matchId, SentProfileId = 1 };
        var expectedMatchDto = new MatchDto { MatchId = matchId, SentProfileId = 1 };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchDto>(match)).Returns(expectedMatchDto);

        // Act
        var result = await _matchService.GetById(matchId);

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
        Assert.ThrowsAsync<KeyNotFoundException>(() => _matchService.GetById(matchId));
    }

    [Test]
    public async Task Add_ValidDto_ReturnsAddedMatchDto()
    {
        // Arrange
        var matchDto = new MatchDto { MatchId = 1, SentProfileId = 1 };
        var match = new MatrimonyApiService.Match.Match { Id = 1, SentProfileId = 1 };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Match.Match>(matchDto)).Returns(match);
        _mockRepo.Setup(repo => repo.Add(match)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchDto>(match)).Returns(matchDto);

        // Act
        var result = await _matchService.Add(matchDto);

        // ClassicAssert
        ClassicAssert.AreEqual(matchDto, result);
    }

    [Test]
    public async Task Update_ValidDto_ReturnsUpdatedMatchDto()
    {
        // Arrange
        var matchDto = new MatchDto { MatchId = 1, SentProfileId = 1 };
        var match = new MatrimonyApiService.Match.Match { Id = 1, SentProfileId = 1 };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Match.Match>(matchDto)).Returns(match);
        _mockRepo.Setup(repo => repo.Update(match)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchDto>(match)).Returns(matchDto);

        // Act
        var result = await _matchService.Update(matchDto);

        // ClassicAssert
        ClassicAssert.AreEqual(matchDto, result);
    }

    [Test]
    public async Task DeleteById_ValidId_ReturnsDeletedMatchDto()
    {
        // Arrange
        var matchId = 1;
        var match = new MatrimonyApiService.Match.Match { Id = matchId, SentProfileId = 1 };
        var expectedMatchDto = new MatchDto { MatchId = matchId, SentProfileId = 1 };

        _mockRepo.Setup(repo => repo.DeleteById(matchId)).ReturnsAsync(match);
        _mockMapper.Setup(mapper => mapper.Map<MatchDto>(match)).Returns(expectedMatchDto);

        // Act
        var result = await _matchService.DeleteById(matchId);

        // ClassicAssert
        ClassicAssert.AreEqual(expectedMatchDto, result);
    }
}