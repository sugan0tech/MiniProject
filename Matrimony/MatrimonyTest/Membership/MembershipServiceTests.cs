using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Membership;
using Microsoft.Extensions.Logging;
using Moq;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;
using NUnit.Framework.Legacy;
using MembershipService = MatrimonyApiService.Membership.MembershipService;

namespace MatrimonyTest.Membership;

[TestFixture]
public class MembershipServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Membership.Membership>> _mockRepo;
    private Mock<IProfileService> _mockProflieService;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<MembershipService>> _mockLogger;
    private MembershipService _membershipService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.Membership.Membership>>();
        _mockProflieService = new Mock<IProfileService>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MembershipService>>();
        _membershipService =
            new MembershipService(_mockRepo.Object, _mockProflieService.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetByPersonId_ValidPersonId_ReturnsMembershipDto()
    {
        // Arrange
        var personId = 1;
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = 1, ProfileId = personId, Type = "Premium", Description = "Test Description" };
        var membershipDto = new MembershipDto
            { MembershipId = 1, ProfileId = personId, Type = "Premium", Description = "Test Description" };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.Membership.Membership>
            { membership });
        _mockMapper.Setup(mapper => mapper.Map<MembershipDto>(membership)).Returns(membershipDto);

        // Act
        var result = await _membershipService.GetByProfileId(personId);

        // ClassicAssert
        ClassicAssert.AreEqual(membershipDto, result);
    }

    [Test]
    public void GetByPersonId_InvalidPersonId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var personId = 1;
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.Membership.Membership>());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipService.GetByProfileId(personId));
    }


    [Test]
    public async Task GetByUserId_validUserId_ReturnsMembership()
    {
        // Arrange
        var personId = 1;
        var userId = 1;
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = 1, ProfileId = personId, Type = "Premium", Description = "Test Description" };
        var membershipDto = new MembershipDto
            { MembershipId = 1, ProfileId = personId, Type = "Premium", Description = "Test Description" };

        var profile = new ProfileDto
        {
            ProfileId = personId,
            DateOfBirth = new DateTime(1990, 5, 15),
            Education = "NoEducation",
            Occupation = "Engineer",
            MotherTongue = "English",
            Religion = "Christian",
            Height = 175,
            MaritalStatus = MaritalStatus.Single.ToString(),
            Ethnicity = Ethnicity.Indian.ToString(),
            Habit = Habit.Cooking.ToString(),
            Gender = Gender.Male.ToString(),
            ManagedByRelation = Relation.Self.ToString(),
            MembershipId = membership.Id
        };


        _mockProflieService.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(profile);
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.Membership.Membership>
            { membership });
        _mockMapper.Setup(mapper => mapper.Map<MembershipDto>(membership)).Returns(membershipDto);

        // Act
        var result = await _membershipService.GetByUserId(userId);

        // ClassicAssert
        ClassicAssert.AreEqual(membershipDto, result);
    }

    [Test]
    public void GetByUserId_InvalidUserId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var personId = 1;
        _mockProflieService.Setup(service => service.GetProfileByUserId(It.IsAny<int>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipService.GetByProfileId(personId));
    }

    [Test]
    public async Task DeleteById_ValidMembershipId_ReturnsDeletedMembershipDto()
    {
        // Arrange
        var membershipId = 1;
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = membershipId, ProfileId = 1, Type = "Premium", Description = "Test Description" };
        var membershipDto = new MembershipDto
        {
            MembershipId = membershipId, ProfileId = 1, Type = "Premium", Description = "Test Description",
            IsTrail = true, IsTrailEnded = false, ViewsCount = 1, ChatCount = 1, RequestCount = 1, ViewersViewCount = 1
        };

        _mockRepo.Setup(repo => repo.DeleteById(membershipId)).ReturnsAsync(membership);
        _mockMapper.Setup(mapper => mapper.Map<MembershipDto>(membership)).Returns(membershipDto);

        // Act
        var result = await _membershipService.DeleteById(membershipId);

        // ClassicAssert
        ClassicAssert.AreEqual(membershipDto, result);
    }

    [Test]
    public void DeleteById_InvalidMembershipId_ThrowsException()
    {
        // Arrange
        var membershipId = 1;
        _mockRepo.Setup(repo => repo.DeleteById(membershipId))
            .Throws(new KeyNotFoundException("Error deleting membership"));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _membershipService.DeleteById(membershipId));
        ClassicAssert.AreEqual("Error deleting membership", ex.Message);
    }

    [Test]
    public async Task Add_ValidDto_ReturnsAddedMembershipDto()
    {
        // Arrange
        var membershipDto = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Membership.Membership>(membershipDto))
            .Returns(membership);
        _mockRepo.Setup(repo => repo.Add(membership)).ReturnsAsync(membership);
        _mockMapper.Setup(mapper => mapper.Map<MembershipDto>(membership)).Returns(membershipDto);

        // Act
        var result = await _membershipService.Add(membershipDto);

        // ClassicAssert
        ClassicAssert.AreEqual(membershipDto, result);
    }

    [Test]
    public void Add_ThrowsException_ReturnsException()
    {
        // Arrange
        var membershipDto = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };

        var membershipOne =
            new MatrimonyApiService.Membership.Membership
            {
                Id = 1,
                EndsAt = DateTime.Now.AddDays(-1),
                Type = MemberShip.PremiumUser.ToString(),
                ProfileId = 1
            };
        var memberships = new List<MatrimonyApiService.Membership.Membership>
        {
            membershipOne
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(memberships);
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Membership.Membership>(membershipDto))
            .Throws(new Exception("Error adding membership"));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<AlreadyExistingEntityException>(async () =>
            await _membershipService.Add(membershipDto));
        ClassicAssert.AreEqual($"Membership with profile 1 already exists", ex.Message);
    }

    [Test]
    public async Task Update_ValidDto_ReturnsUpdatedMembershipDto()
    {
        // Arrange
        var membershipDto = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };

        _mockRepo.Setup(repo => repo.GetById(membershipDto.MembershipId)).ReturnsAsync(membership);
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Membership.Membership>(membershipDto))
            .Returns(membership);
        _mockRepo.Setup(repo => repo.Update(membership)).ReturnsAsync(membership);
        _mockMapper.Setup(mapper => mapper.Map<MembershipDto>(membership)).Returns(membershipDto);

        // Act
        var result = await _membershipService.Update(membershipDto);

        // ClassicAssert
        ClassicAssert.AreEqual(membershipDto, result);
    }

    [Test]
    public void Update_InvalidDto_ThrowsKeyNotFoundException()
    {
        // Arrange
        var membership = new MatrimonyApiService.Membership.Membership
        {
            Id = 1, ProfileId = 1, Type = MemberShip.PremiumUser.ToString(), Description = "Test Description",
            IsTrail = true
        };

        var membershipDto = new MembershipDto
        {
            MembershipId = 1, ProfileId = 1, Type = MemberShip.PremiumUser.ToString(), Description = "Test Description",
            IsTrail = true
        };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Membership.Membership>(membershipDto))
            .Returns(membership);
        _mockRepo.Setup(repo => repo.Update(membership)).Throws(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipService.Update(membershipDto));
    }

    [Test]
    public void Update_ThrowsException_ReturnsException()
    {
        // Arrange
        var membershipDto = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = 1, ProfileId = 1, Type = "Premium", Description = "Test Description" };

        _mockRepo.Setup(repo => repo.GetById(membershipDto.MembershipId)).ReturnsAsync(membership);
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Membership.Membership>(membershipDto))
            .Throws(new Exception("Error updating membership"));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _membershipService.Update(membershipDto));
        ClassicAssert.AreEqual($"Error updating membership", ex.Message);
    }

    [Test]
    public async Task Validate_ExpiredMembership_SetsToFreeUser()
    {
        // Arrange
        var membershipId = 1;
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = membershipId, EndsAt = DateTime.Now.AddDays(-1), Type = "Premium", ProfileId = 1 };

        _mockRepo.Setup(repo => repo.GetById(membershipId)).ReturnsAsync(membership);
        _mockRepo.Setup(repo => repo.Update(It.IsAny<MatrimonyApiService.Membership.Membership>()))
            .ReturnsAsync(membership);

        // Act
        await _membershipService.Validate(membershipId);

        // ClassicAssert
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.Membership.Membership>(m => m.TypeEnum == MemberShip.FreeUser)), Times.Once);
    }

    [Test]
    public async Task Validate_TrailEndedMembership_ShouldExpiryTrial()
    {
        // Arrange
        var membershipId = 1;
        var membership = new MatrimonyApiService.Membership.Membership
            { Id = membershipId, EndsAt = DateTime.Now.AddDays(-1), Type = "Premium", ProfileId = 1, IsTrail = true };
        var membershipdto = new MembershipDto
        {
            MembershipId = membershipId, EndsAt = DateTime.Now.AddDays(-1), Type = "Premium", ProfileId = 1,
            IsTrail = true
        };

        _mockRepo.Setup(repo => repo.GetById(membershipId)).ReturnsAsync(membership);
        _mockRepo.Setup(repo => repo.Update(It.IsAny<MatrimonyApiService.Membership.Membership>()))
            .ReturnsAsync(membership);

        // Act
        await _membershipService.Validate(membershipdto);

        // ClassicAssert
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.Membership.Membership>(m => m.IsTrailEnded == true)), Times.Once);
    }

    [Test]
    public void Validate_InvalidMembershipId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var membershipId = 1;
        _mockRepo.Setup(repo => repo.GetById(membershipId)).Throws(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _membershipService.Validate(membershipId));
    }

    [Test]
    public async Task ValidateAll_ValidatesAllMemberships()
    {
        // Arrange
        var membershipOne =
            new MatrimonyApiService.Membership.Membership
            {
                Id = 1,
                EndsAt = DateTime.Now.AddDays(-1),
                Type = MemberShip.PremiumUser.ToString(),
                ProfileId = 1
            };
        var membershipTwo =
            new MatrimonyApiService.Membership.Membership
            {
                Id = 2,
                EndsAt = DateTime.Now.AddDays(1),
                Type = MemberShip.PremiumUser.ToString(),
                ProfileId = 2
            };
        var memberships = new List<MatrimonyApiService.Membership.Membership>
        {
            membershipOne,
            membershipTwo
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(memberships);
        _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(membershipOne);
        _mockRepo.Setup(repo => repo.GetById(2)).ReturnsAsync(membershipTwo);
        _mockRepo.Setup(repo => repo.Update(It.IsAny<MatrimonyApiService.Membership.Membership>()))
            .ReturnsAsync((MatrimonyApiService.Membership.Membership m) => m);

        // Act
        await _membershipService.ValidateAll();

        // ClassicAssert
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.Membership.Membership>(m => m.Id == 1 && m.TypeEnum == MemberShip.FreeUser)),
            Times.Once);
        _mockRepo.Verify(
            repo => repo.Update(
                It.Is<MatrimonyApiService.Membership.Membership>(m =>
                    m.Id == 2 && m.TypeEnum == MemberShip.PremiumUser)), Times.Once);
    }
}