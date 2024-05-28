using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.ProfileView;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.ProfileView;

public class ProfileViewServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.ProfileView.ProfileView>> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<ProfileViewService>> _mockLogger;
    private ProfileViewService _profileViewService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.ProfileView.ProfileView>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<ProfileViewService>>();
        _profileViewService = new ProfileViewService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
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


        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .ReturnsAsync(profileView);

        // Act
        await _profileViewService.AddView(viewerId, profileId);

        // ClassicAssert
        _mockRepo.Verify(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()), Times.Once);
    }

    [Test]
    public void AddView_WithViewerIdAndProfileId_ThrowsException()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 2;

        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.ProfileView.ProfileView>()))
            .Throws(new Exception("Error adding profile view."));

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<Exception>(async () => await _profileViewService.AddView(viewerId, profileId));
        ClassicAssert.AreEqual("Error adding profile view.", ex.Message);
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
}