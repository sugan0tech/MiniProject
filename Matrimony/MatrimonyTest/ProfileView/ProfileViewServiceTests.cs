using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.ProfileView;

public class ProfileViewServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.ProfileView.ProfileView>> _mockRepo;
    private Mock<IProfileService> _mockProfileService;
    private Mock<IMapper> _mockMapper;
    private Mock<IMembershipService> _membershipServiceMock;
    private Mock<IUserService> _mockUserService;
    private Mock<ILogger<ProfileViewService>> _mockLogger;
    private ProfileViewService _profileViewService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.ProfileView.ProfileView>>();
        _mockProfileService = new Mock<IProfileService>();
        _mockMapper = new Mock<IMapper>();
        _mockUserService = new Mock<IUserService>();
        _membershipServiceMock = new Mock<IMembershipService>();
        _mockLogger = new Mock<ILogger<ProfileViewService>>();
        _profileViewService = new ProfileViewService(_mockRepo.Object, _membershipServiceMock.Object,
            _mockUserService.Object,
            _mockProfileService.Object,
            _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task AddView_WithViewerIdAndProfileId_AddsProfileView()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 2;
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            ViewerId = viewerId,
            ViewedProfileAt = profileId,
            ViewedAt = DateTime.Now
        };

        var membership = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = "Premium", Description = "Test Description", IsTrail = true };

        _membershipServiceMock.Setup(service => service.GetByProfileId(1)).ReturnsAsync(membership);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .ReturnsAsync(profileView);
        _mockProfileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(new ProfileDto { });

        // Act
        await _profileViewService.AddView(viewerId, profileId);

        // ClassicAssert
        _mockRepo.Verify(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()), Times.Once);
    }

    [Test]
    public async Task AddView_WithViewerIdAndProfileIdFreeUser_ThrowsNonPremiumUserException()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 2;
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            ViewerId = viewerId,
            ViewedProfileAt = profileId,
            ViewedAt = DateTime.Now
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

        var membership = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = MemberShip.FreeUser.ToString(), Description = "Test Description", IsTrail = false };

        _membershipServiceMock.Setup(service => service.GetByProfileId(1)).ReturnsAsync(membership);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .ReturnsAsync(profileView);
        _mockProfileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(profile);

        // Act&Assert
        Assert.ThrowsAsync<NonPremiumUserException>(async () => await _profileViewService.AddView(viewerId, profileId));
    }

    [Test]
    public async Task AddView_WithViewerIdAndProfileIdBasicUser_ThrowsExhaustedMaxNoOfViewsException()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 2;
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            ViewerId = viewerId,
            ViewedProfileAt = profileId,
            ViewedAt = DateTime.Now
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

        var membership = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = MemberShip.BasicUser.ToString(), Description = "Test Description", IsTrail = false, ViewsCount = 50 };

        _membershipServiceMock.Setup(service => service.GetByProfileId(1)).ReturnsAsync(membership);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .ReturnsAsync(profileView);
        _mockProfileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(profile);

        // Act&Assert
        Assert.ThrowsAsync<ExhaustedMaximumProfileViewsException>(async () => await _profileViewService.AddView(viewerId, profileId));
    }

    [Test]
    public async Task AddView_WithViewerIdAndProfileIdBasicUser_AddsView()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 2;
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            ViewerId = viewerId,
            ViewedProfileAt = profileId,
            ViewedAt = DateTime.Now
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

        var membership = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = MemberShip.BasicUser.ToString(), Description = "Test Description", IsTrail = false, ViewsCount = 49 };

        _membershipServiceMock.Setup(service => service.GetByProfileId(1)).ReturnsAsync(membership);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .ReturnsAsync(profileView);
        _mockProfileService.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(profile);

        // Act
        await _profileViewService.AddView(viewerId, profileId);

        // ClassicAssert
        _mockRepo.Verify(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()), Times.Once);
    }

    [Test]
    public void AddView_WithViewerIdAndProfileId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 2;

        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .Throws(new Exception("Error adding profile view."));

        _mockProfileService.Setup(service => service.GetProfileById(profileId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileViewService.AddView(viewerId, profileId));
        ClassicAssert.AreEqual("The given key was not present in the dictionary.", ex.Message);
    }

    [Test]
    public async Task AddView_WithProfileViewDto_AddsProfileView()
    {
        // Arrange
        var profileViewDto = new ProfileViewDto
        {
            ProfileViewId = 1,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            Id = 1,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.ProfileView.ProfileView>(profileViewDto))
            .Returns(profileView);
        _mockRepo.Setup(repo => repo.Add(profileView)).ReturnsAsync(profileView);

        // Act
        await _profileViewService.AddView(profileViewDto);

        // ClassicAssert
        _mockRepo.Verify(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()), Times.Once);
    }

    [Test]
    public void AddView_WithProfileViewDto_ThrowsException()
    {
        // Arrange
        var profileViewDto = new ProfileViewDto
        {
            ProfileViewId = 1,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.ProfileView.ProfileView>(profileViewDto))
            .Throws(new Exception("Error adding profile view."));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _profileViewService.AddView(profileViewDto));
        ClassicAssert.AreEqual("Error adding profile view.", ex.Message);
    }

    [Test]
    public async Task GetViewById_ValidId_ReturnsProfileViewDto()
    {
        // Arrange
        var viewId = 1;
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            Id = viewId,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };
        var profileViewDto = new ProfileViewDto
        {
            ProfileViewId = viewId,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };

        _mockRepo.Setup(repo => repo.GetById(viewId)).ReturnsAsync(profileView);
        _mockMapper.Setup(mapper => mapper.Map<ProfileViewDto>(profileView)).Returns(profileViewDto);

        // Act
        var result = await _profileViewService.GetViewById(viewId);

        // ClassicAssert
        ClassicAssert.AreEqual(profileViewDto, result);
    }

    [Test]
    public void GetViewById_InvalidId_ThrowsException()
    {
        // Arrange
        var viewId = 1;
        _mockRepo.Setup(repo => repo.GetById(viewId))
            .Throws(new KeyNotFoundException("Error getting profile view with id 1."));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileViewService.GetViewById(viewId));
        ClassicAssert.AreEqual("Error getting profile view with id 1.", ex.Message);
    }

    [Test]
    public async Task GetViewsByProfileId_ValidProfileId_ReturnsProfileViews()
    {
        // Arrange
        var profileId = 1;
        var views = new List<MatrimonyApiService.ProfileView.ProfileView>
        {
            new() { Id = 1, ViewerId = 1, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-10) },
            new() { Id = 2, ViewerId = 2, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-20) },
            new() { Id = 3, ViewerId = 3, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-30) }
        };

        var viewDtos = views.ConvertAll(view => new ProfileViewDto
        {
            ProfileViewId = view.Id,
            ViewerId = view.ViewerId,
            ViewedProfileAt = view.ViewedProfileAt,
            ViewedAt = view.ViewedAt
        });
        
        var membership = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = MemberShip.PremiumUser.ToString(), Description = "Test Description", IsTrail = false, ViewsCount = 49 };

        _membershipServiceMock.Setup(service => service.GetByProfileId(1)).ReturnsAsync(membership);

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(views);
        _mockMapper.Setup(mapper => mapper.Map<ProfileViewDto>(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .Returns((MatrimonyApiService.ProfileView.ProfileView src) => new ProfileViewDto
            {
                ProfileViewId = src.Id,
                ViewerId = src.ViewerId,
                ViewedProfileAt = src.ViewedProfileAt,
                ViewedAt = src.ViewedAt
            });

        // Act
        var result = await _profileViewService.GetViewsByProfileId(profileId);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(views.Count, result.Count);
        for (var i = 0; i < views.Count; i++)
        {
            ClassicAssert.AreEqual(viewDtos[i].ProfileViewId, result[i].ProfileViewId);
            ClassicAssert.AreEqual(viewDtos[i].ViewerId, result[i].ViewerId);
            ClassicAssert.AreEqual(viewDtos[i].ViewedProfileAt, result[i].ViewedProfileAt);
            ClassicAssert.AreEqual(viewDtos[i].ViewedAt, result[i].ViewedAt);
        }
    }

    
    [Test]
    public async Task GetViewsByProfileId_BasicUser_ReturnsOnlyFive()
    {
        // Arrange
        var profileId = 1;
        var views = new List<MatrimonyApiService.ProfileView.ProfileView>
        {
            new() { Id = 1, ViewerId = 1, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-10) },
            new() { Id = 2, ViewerId = 2, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-20) },
            new() { Id = 3, ViewerId = 3, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-30) },
            new() { Id = 4, ViewerId = 4, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-30) },
            new() { Id = 5, ViewerId = 5, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-30) },
            new() { Id = 6, ViewerId = 5, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddDays(-30) }
        };

        var viewDtos = views.ConvertAll(view => new ProfileViewDto
        {
            ProfileViewId = view.Id,
            ViewerId = view.ViewerId,
            ViewedProfileAt = view.ViewedProfileAt,
            ViewedAt = view.ViewedAt
        });
        
        var membership = new MembershipDto
            { MembershipId = 1, ProfileId = 1, Type = MemberShip.BasicUser.ToString(), Description = "Test Description", IsTrail = false, ViewsCount = 45 };

        _membershipServiceMock.Setup(service => service.GetByProfileId(1)).ReturnsAsync(membership);

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(views);
        _mockMapper.Setup(mapper => mapper.Map<ProfileViewDto>(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .Returns((MatrimonyApiService.ProfileView.ProfileView src) => new ProfileViewDto
            {
                ProfileViewId = src.Id,
                ViewerId = src.ViewerId,
                ViewedProfileAt = src.ViewedProfileAt,
                ViewedAt = src.ViewedAt
            });

        // Act
        var result = await _profileViewService.GetViewsByProfileId(profileId);

        // Assert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(5, result.Count);
    }
    [Test]
    public async Task DeleteViewById_ValidId_DeletesProfileView()
    {
        // Arrange
        var viewId = 1;
        var profileView = new MatrimonyApiService.ProfileView.ProfileView
        {
            Id = viewId,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };
        var profileViewDto = new ProfileViewDto
        {
            ProfileViewId = viewId,
            ViewerId = 1,
            ViewedProfileAt = 2,
            ViewedAt = DateTime.Now
        };
        _mockRepo.Setup(repo => repo.DeleteById(viewId)).ReturnsAsync(profileView);

        // Act
        await _profileViewService.DeleteViewById(viewId);

        // ClassicAssert
        _mockRepo.Verify(repo => repo.DeleteById(viewId), Times.Once);
    }

    [Test]
    public void DeleteViewById_InvalidId_ThrowsException()
    {
        // Arrange
        var viewId = 1;
        _mockRepo.Setup(repo => repo.DeleteById(viewId))
            .Throws(new KeyNotFoundException("Error deleting profile view with id 1."));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _profileViewService.DeleteViewById(viewId));
        ClassicAssert.AreEqual("Error deleting profile view with id 1.", ex.Message);
    }

    [Test]
    public async Task DeleteOldViews_BeforeDate_DeletesOldProfileViews()
    {
        // Arrange
        var beforeDate = DateTime.Now.AddMonths(-1);
        var views = new List<MatrimonyApiService.ProfileView.ProfileView>
        {
            new() { Id = 1, ViewerId = 1, ViewedProfileAt = 2, ViewedAt = DateTime.Now.AddMonths(-2) },
            new() { Id = 2, ViewerId = 2, ViewedProfileAt = 3, ViewedAt = DateTime.Now.AddDays(-10) }
        };
        var resProfileView = new MatrimonyApiService.ProfileView.ProfileView
            { Id = 1, ViewerId = 1, ViewedProfileAt = 2, ViewedAt = DateTime.Now.AddDays(-2) };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(views);
        _mockRepo.Setup(repo => repo.DeleteById(It.IsAny<int>())).ReturnsAsync(resProfileView);

        // Act
        await _profileViewService.DeleteOldViews(beforeDate);

        // ClassicAssert
        _mockRepo.Verify(repo => repo.DeleteById(1), Times.Once);
        _mockRepo.Verify(repo => repo.DeleteById(2), Times.Never);
    }

    [Test]
    public void DeleteOldViews_ThrowsException()
    {
        // Arrange
        var beforeDate = DateTime.Now.AddMonths(-1);
        _mockRepo.Setup(repo => repo.GetAll()).Throws(new KeyNotFoundException("Error deleting old profile views."));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _profileViewService.DeleteOldViews(beforeDate));
        ClassicAssert.AreEqual("Error deleting old profile views.", ex.Message);
    }

    [Test]
    public void DeleteOldViews_InvalidBeforeDate_ThrowsInvalidDateTimeException()
    {
        // Arrange
        var beforeDate = DateTime.Now.AddMonths(+1);

        // Act & ClassicAssert
        Assert.ThrowsAsync<InvalidDateTimeException>(async () => await _profileViewService.DeleteOldViews(beforeDate));
    }

    [Test]
    public async Task GetViews_ShouldReturnProfileViews()
    {
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
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
            RelationEnum = Relation.Friend,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        var user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        var membershipDto = new MembershipDto
        {
            MembershipId = 1, Type = MemberShip.PremiumUser.ToString(), ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        var membership = new MatrimonyApiService.Membership.Membership
        {
            Id = 1, Type = MemberShip.PremiumUser.ToString(), ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };
        profile.Membership = membership;
        profile.MembershipId = 1;

        var profileId = 1;
        var profileViews = new List<MatrimonyApiService.ProfileView.ProfileView>
        {
            new() { Id = 1, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddMonths(-2) },
            new() { Id = 2, ViewedProfileAt = profileId, ViewedAt = DateTime.Now.AddMonths(-1) }
        };

        var profileView = new MatrimonyApiService.ProfileView.ProfileView
            { Id = 1, ViewerId = 1, ViewedProfileAt = 2, ViewedAt = DateTime.Now.AddDays(-2) };

        _mockRepo.Setup(r => r.GetById(profileId)).ReturnsAsync(profileView);
        _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(profileViews);
        _membershipServiceMock.Setup(p => p.GetByProfileId(profileId)).ReturnsAsync(membershipDto);

        var result = await _profileViewService.GetViewsByProfileId(profileId);

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public void GetViews_ShouldThrowNonPremiumUserException_WhenMembershipIsFree()
    {
        var profile = new MatrimonyApiService.Profile.Profile
        {
            Id = 1,
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
        var user = new MatrimonyApiService.User.User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = "password"u8.ToArray(),
            HashKey = "key"u8.ToArray()
        };
        var membership = new MembershipDto
        {
            Type = MemberShip.FreeUser.ToString(), ProfileId = 1,
            Description = "Premium membership", EndsAt = DateTime.Now.AddMonths(1), IsTrail = false
        };

        var profileView = new MatrimonyApiService.ProfileView.ProfileView
            { Id = 1, ViewerId = 1, ViewedProfileAt = 2, ViewedAt = DateTime.Now.AddDays(-2) };

        var profileViews = new List<MatrimonyApiService.ProfileView.ProfileView>
        {
            new() { Id = 1, ViewedProfileAt = 1, ViewedAt = DateTime.Now.AddMonths(-2) },
            new() { Id = 2, ViewedProfileAt = 1, ViewedAt = DateTime.Now.AddMonths(-1) }
        };

        var profileId = 1;
        _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(profileViews);
        _mockRepo.Setup(r => r.GetById(profileId)).ReturnsAsync(profileView);
        _membershipServiceMock.Setup(m => m.GetByProfileId(1)).ReturnsAsync(membership);

        Assert.ThrowsAsync<NonPremiumUserException>(() => _profileViewService.GetViewsByProfileId(profileId));
    }
}