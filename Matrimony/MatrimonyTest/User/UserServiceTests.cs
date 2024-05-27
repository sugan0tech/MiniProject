using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using MatrimonyApiService.Profile;
using MatrimonyApiService.ProfileView;
using MatrimonyApiService.User;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.User;

public class UserServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.User.User>> _userRepositoryMock;
    private Mock<IProfileService> _profileServiceMock;
    private Mock<IProfileViewService> _profileViewServiceMock;
    private Mock<IMembershipService> _membershipServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<UserService>> _loggerMock;
    private UserService userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IBaseRepo<MatrimonyApiService.User.User>>();
        _profileServiceMock = new Mock<IProfileService>();
        _profileViewServiceMock = new Mock<IProfileViewService>();
        _membershipServiceMock = new Mock<IMembershipService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();
        userService = new UserService(
            _userRepositoryMock.Object,
            _profileServiceMock.Object,
            _profileViewServiceMock.Object,
            _membershipServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task ViewProfile_ValidUser_ReturnsProfileDto()
    {
        // Arrange
        var userId = 1;
        var profileId = 2;
        var user = new MatrimonyApiService.User.User
        {
            Id = userId,
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = new byte[]
            {
                // Populate password bytes here
            },
            HashKey = new byte[]
            {
                // Populate hash key bytes here
            }
        };
        var userProfileDto = new ProfileDto { ProfileId = profileId };
        var membership = new MembershipDto { Type = MemberShip.PremiumUser.ToString() };

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(userProfileDto);
        _membershipServiceMock.Setup(service => service.GetByProfileId(profileId)).ReturnsAsync(membership);
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(userProfileDto);

        // Act
        var result = await userService.ViewProfile(userId, profileId);

        // ClassicAssert
        ClassicAssert.AreEqual(userProfileDto, result);
    }

    [Test]
    public void ViewProfile_NonPremiumUser_ThrowsNonPremiumUserException()
    {
        // Arrange


        var userId = 1;
        var profileId = 2;
        var user = new MatrimonyApiService.User.User
        {
            Id = userId,
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = new byte[]
            {
                // Populate password bytes here
            },
            HashKey = new byte[]
            {
                // Populate hash key bytes here
            }
        };
        var userProfileDto = new ProfileDto { ProfileId = profileId };
        var membership = new MembershipDto { Type = MemberShip.FreeUser.ToString() };

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(userProfileDto);
        _membershipServiceMock.Setup(service => service.GetByProfileId(profileId)).ReturnsAsync(membership);

        // Act & ClassicAssert
        Assert.ThrowsAsync<NonPremiumUserException>(() => userService.ViewProfile(userId, profileId));
    }

    [Test]
    public async Task ViewProfile_BasicUserBelowViewLimit_ReturnsProfileDto()
    {
        // Arrange
;

        var userId = 1;
        var profileId = 2;
        var user = new MatrimonyApiService.User.User
        {
            Id = userId,
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = new byte[]
            {
                // Populate password bytes here
            },
            HashKey = new byte[]
            {
                // Populate hash key bytes here
            }
        };
        var userProfileDto = new ProfileDto { ProfileId = profileId };
        var membership = new MembershipDto { Type = MemberShip.BasicUser.ToString() };

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(userProfileDto);
        _membershipServiceMock.Setup(service => service.GetByProfileId(profileId)).ReturnsAsync(membership);
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(userProfileDto);
        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);

        // Act
        var result = await userService.ViewProfile(userId, profileId);

        // ClassicAssert
        ClassicAssert.AreEqual(userProfileDto, result);
    }

    [Test]
    public void ViewProfile_BasicUserExceedsViewLimit_ThrowsExhaustedMaximumProfileViewsException()
    {
        // Arrange


        var userId = 1;
        var profileId = 2;
        var views = new List<MatrimonyApiService.ProfileView.ProfileView>();
        for (var i = 0; i < 51; i++) views.Add(new MatrimonyApiService.ProfileView.ProfileView
        {
            Id = i,
            ViewerId = 1,
            ViewedProfileAt = 1,
            ViewedAt = DateTime.Now
        });
        var user = new MatrimonyApiService.User.User
        {
            Id = userId,
            Email = null,
            FirstName = null,
            LastName = null,
            PhoneNumber = null,
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            },
            Views = views
        };
        var userProfileDto = new ProfileDto { ProfileId = profileId };
        var membership = new MembershipDto { Type = MemberShip.BasicUser.ToString() };


        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(userProfileDto);
        _membershipServiceMock.Setup(service => service.GetByProfileId(profileId)).ReturnsAsync(membership);

        // Act & ClassicAssert
        Assert.ThrowsAsync<ExhaustedMaximumProfileViewsException>(() => userService.ViewProfile(userId, profileId));
    }

    [Test]
    public void ViewProfile_UserNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange

        var userId = 1;
        var profileId = 2;

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(() => userService.ViewProfile(userId, profileId));
    }
}
