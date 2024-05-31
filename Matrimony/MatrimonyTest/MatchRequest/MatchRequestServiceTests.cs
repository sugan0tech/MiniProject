using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Profile;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.MatchRequest;

public class MatchRequestServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.MatchRequest.MatchRequest>> _mockRepo;
    private Mock<IProfileService> _mockProfileService;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<MatchRequestService>> _mockLogger;
    private MatchRequestService _matchRequestService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.MatchRequest.MatchRequest>>();
        _mockProfileService = new Mock<IProfileService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MatchRequestService>>();
        _matchRequestService = new MatchRequestService(_mockRepo.Object, _mockProfileService.Object,
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
        var result = await _matchRequestService.GetAcceptedMatcheRequests(profileId);

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
        var result = await _matchRequestService.GetMatchRequests(profileId);

        // ClassicAssert
        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.AreEqual(expectedMatches[0].MatchId, result[0].MatchId);
        ClassicAssert.AreEqual(expectedMatches[0].ReceivedProfileId, result[0].ReceivedProfileId);
        ClassicAssert.AreEqual(expectedMatches[1].MatchId, result[1].MatchId);
        ClassicAssert.AreEqual(expectedMatches[1].ReceivedProfileId, result[1].ReceivedProfileId);
    }

    [Test]
    public async Task GetSentMatchRequests_ValidProfileId_ReturnsSentMatchRequests()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<MatchRequestDto>
        {
            new() { MatchId = 1, SentProfileId = profileId },
            new() { MatchId = 2, SentProfileId = 2 },
            new() { MatchId = 3, SentProfileId = profileId }
        };
        
        var profile = new ProfileDto
        {
            ProfileId = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _mockProfileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(profile);
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.MatchRequest.MatchRequest>());
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>()))
            .Returns((MatrimonyApiService.MatchRequest.MatchRequest source) => new MatchRequestDto
                { MatchId = source.Id, SentProfileId = source.SentProfileId });

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.MatchRequest.MatchRequest>());
        _matchRequestService = new MatchRequestService(_mockRepo.Object, _mockProfileService.Object, _mockMapper.Object, _mockLogger.Object);
            
        // Act
        var result = await _matchRequestService.GetSentMatchRequests(profileId);

        // ClassicAssert
        ClassicAssert.AreEqual(0, result.Count);
    }

    [Test]
    public async Task Approve_ValidMatchIdAndProfileId_UpdatesMatch()
    {
        // Arrange
        var matchId = 1;
        var profileId = 2;
        var match = new MatrimonyApiService.MatchRequest.MatchRequest
            { Id = matchId, ReceivedProfileId = profileId, ProfileTwoLike = false };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);

        // Act
        await _matchRequestService.Approve(matchId, profileId);

        // ClassicAssert
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.MatchRequest.MatchRequest>(m => m.Id == matchId && m.ProfileTwoLike == true)), Times.Once);
    }

    [Test]
    public void Approve_InvalidMatchId_ThrowsInvalidMatchForProfile()
    {
        // Arrange
        var matchId = 1;
        var profileId = 3;
        var match = new MatrimonyApiService.MatchRequest.MatchRequest { Id = matchId, ReceivedProfileId = 2 };

        _mockRepo.Setup(repo => repo.GetById(matchId)).ReturnsAsync(match);

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<InvalidMatchForProfile>(
            () => _matchRequestService.Approve(matchId, profileId));
        ClassicAssert.AreEqual($"The match {matchId} is not meant for {profileId}", ex.Message);
    }

    [Test]
    public async Task MatchRequestToProfile_ValidRequest_CreatesMatchRequest()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        var newMatch = new MatchRequestDto { SentProfileId = senderId, ReceivedProfileId = targetId, FoundAt = DateTime.Now };
        var newMatchEntity = new MatrimonyApiService.MatchRequest.MatchRequest { SentProfileId = senderId, ReceivedProfileId = targetId, FoundAt = DateTime.Now };
        var profile = new ProfileDto
        {
            ProfileId = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var profileTwo = new ProfileDto
        {
            ProfileId = 2,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _mockProfileService.Setup(service => service.GetProfileById(senderId)).ReturnsAsync(profile);
        _mockProfileService.Setup(service => service.GetProfileById(targetId)).ReturnsAsync(profileTwo);
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.MatchRequest.MatchRequest>());
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.MatchRequest.MatchRequest>(It.IsAny<MatchRequestDto>()))
            .Returns(newMatchEntity);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>())).ReturnsAsync(newMatchEntity);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>()))
            .Returns(newMatch);

        // Act
        var result = await _matchRequestService.MatchRequestToProfile(senderId, targetId);

        // ClassicAssert
        ClassicAssert.AreEqual(newMatch.SentProfileId, result.SentProfileId);
        ClassicAssert.AreEqual(newMatch.ReceivedProfileId, result.ReceivedProfileId);
        ClassicAssert.AreEqual(newMatch.FoundAt, result.FoundAt);
    }

    
    [Test]
    public async Task MatchRequestToProfile_InvalidRequest_ThrowsDuplicateRequestExeption()
    {
        // Arrange
        var senderId = 1;
        var targetId = 2;
        var newMatch = new MatchRequestDto { SentProfileId = senderId, ReceivedProfileId = targetId, FoundAt = DateTime.Now };
        var newMatchEntity = new MatrimonyApiService.MatchRequest.MatchRequest { SentProfileId = senderId, ReceivedProfileId = targetId, FoundAt = DateTime.Now };
        var profile = new ProfileDto
        {
            ProfileId = 1,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var profileTwo = new ProfileDto
        {
            ProfileId = 2,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            AnnualIncome = 50000,
            Occupation = "Engineer",
            MaritalStatus = "Single",
            MotherTongue = "English",
            Religion = "Christian",
            Ethnicity = "Indian",
            Habit = "PetLover",
            Gender = "Male",
            Weight = 70,
            Height = 175,
            MembershipId = 1,
            ManagedById = 1,
            UserId = 1,
            ManagedByRelation = "Friend",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var profileId = 1;
        var matches = new List<MatrimonyApiService.MatchRequest.MatchRequest>
        {
            new() { Id = 1, ReceivedProfileId = profileId },
            new() { Id = 2, ReceivedProfileId = 2 , ProfileOneLike = true}
        };
        var expectedMatches = new List<MatchRequestDto>
        {
            new() { MatchId = 1, ReceivedProfileId = profileId },
            new() { MatchId = 3, ReceivedProfileId = profileId }
        };

        _mockProfileService.Setup(service => service.GetProfileById(senderId)).ReturnsAsync(profile);
        _mockProfileService.Setup(service => service.GetProfileById(targetId)).ReturnsAsync(profileTwo);
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(matches);
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.MatchRequest.MatchRequest>(It.IsAny<MatchRequestDto>()))
            .Returns(newMatchEntity);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>())).ReturnsAsync(newMatchEntity);
        _mockMapper.Setup(mapper => mapper.Map<MatchRequestDto>(It.IsAny<MatrimonyApiService.MatchRequest.MatchRequest>()))
            .Returns(newMatch);

        // Act & Assert
        var ex = Assert.ThrowsAsync<DuplicateRequestException>(
            () => _matchRequestService.MatchRequestToProfile(senderId, targetId));
        ClassicAssert.AreEqual($"You have already sent request for this Profile {targetId}", ex.Message);
    }
    [Test]
    public void MatchRequestToProfile_SameSenderAndTarget_ThrowsMatchRequestToSelfException()
    {
        // Arrange
        var senderId = 1;
        var targetId = 1;

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<MatchRequestToSelfException>(
            () => _matchRequestService.MatchRequestToProfile(senderId, targetId));
        ClassicAssert.AreEqual($"{senderId} is trying to give self request", ex.Message);
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
        await _matchRequestService.Reject(matchId, profileId);

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
            () => _matchRequestService.Reject(matchId, profileId));
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
        var result = await _matchRequestService.GetById(matchId);

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
        Assert.ThrowsAsync<KeyNotFoundException>(() => _matchRequestService.GetById(matchId));
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
        var result = await _matchRequestService.DeleteById(matchId);

        // ClassicAssert
        ClassicAssert.AreEqual(expectedMatchDto, result);
    }
    

    [Test]
    public async Task DeleteById_InValidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var matchId = 1;

        _mockRepo.Setup(repo => repo.DeleteById(matchId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(
            () => _matchRequestService.DeleteById(matchId));
    }
}