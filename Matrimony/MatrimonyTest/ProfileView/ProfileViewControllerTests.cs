using MatrimonyApiService.Exceptions;
using MatrimonyApiService.ProfileView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.ProfileView;

public class ProfileViewControllerTests
{
    private Mock<IProfileViewService> _profileViewServiceMock;
    private Mock<ILogger<ProfileViewController>> _loggerMock;
    private ProfileViewController _profileViewController;

    [SetUp]
    public void SetUp()
    {
        _profileViewServiceMock = new Mock<IProfileViewService>();
        _loggerMock = new Mock<ILogger<ProfileViewController>>();
        _profileViewController = new ProfileViewController(_profileViewServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task AddView_ByIds_ReturnsOk_WhenViewIsAdded()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 1;

        // Act
        var result = await _profileViewController.AddView(viewerId, profileId) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task AddView_ByIds_ReturnsNotFound_WhenProfileOrViewerNotFound()
    {
        // Arrange
        var viewerId = 1;
        var profileId = 1;
        _profileViewServiceMock.Setup(service => service.AddView(viewerId, profileId)).ThrowsAsync(new KeyNotFoundException("Not found"));

        // Act
        var result = await _profileViewController.AddView(viewerId, profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task AddView_ByDto_ReturnsOk_WhenViewIsAdded()
    {
        // Arrange
        var profileViewDto = new ProfileViewDto { ProfileViewId = 1, ViewerId = 1, ViewedProfileAt = 1 };

        // Act
        var result = await _profileViewController.AddView(profileViewDto) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task GetViewById_ReturnsOk_WhenViewExists()
    {
        // Arrange
        var viewId = 1;
        var profileViewDto = new ProfileViewDto { ProfileViewId = 1, ViewerId = 1, ViewedProfileAt = 1 };
        _profileViewServiceMock.Setup(service => service.GetViewById(viewId)).ReturnsAsync(profileViewDto);

        // Act
        var result = await _profileViewController.GetViewById(viewId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileViewDto, result.Value);
    }

    [Test]
    public async Task GetViewById_ReturnsNotFound_WhenViewDoesNotExist()
    {
        // Arrange
        var viewId = 1;
        _profileViewServiceMock.Setup(service => service.GetViewById(viewId)).ThrowsAsync(new KeyNotFoundException("Not found"));

        // Act
        var result = await _profileViewController.GetViewById(viewId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetViewsByProfileId_ReturnsOk_WhenViewsExist()
    {
        // Arrange
        var profileId = 1;
        var profileViews = new List<ProfileViewDto> { new ProfileViewDto { ProfileViewId = 1, ViewedProfileAt = profileId } };
        _profileViewServiceMock.Setup(service => service.GetViewsByProfileId(profileId)).ReturnsAsync(profileViews);

        // Act
        var result = await _profileViewController.GetViewsByProfileId(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileViews, result.Value);
    }

    [Test]
    public async Task GetViewsByProfileId_ReturnsNotFound_WhenNoViewsExist()
    {
        // Arrange
        var profileId = 1;
        _profileViewServiceMock.Setup(service => service.GetViewsByProfileId(profileId)).ThrowsAsync(new KeyNotFoundException("Not found"));

        // Act
        var result = await _profileViewController.GetViewsByProfileId(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetViewsByProfileId_ReturnsForbidden_WhenUserIsNotPremium()
    {
        // Arrange
        var profileId = 1;
        _profileViewServiceMock.Setup(service => service.GetViewsByProfileId(profileId)).ThrowsAsync(new NonPremiumUserException("Forbidden"));

        // Act
        var result = await _profileViewController.GetViewsByProfileId(profileId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task DeleteViewById_ReturnsOk_WhenViewIsDeleted()
    {
        // Arrange
        var viewId = 1;

        // Act
        var result = await _profileViewController.DeleteViewById(viewId) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task DeleteViewById_ReturnsNotFound_WhenViewDoesNotExist()
    {
        // Arrange
        var viewId = 1;
        _profileViewServiceMock.Setup(service => service.DeleteViewById(viewId)).ThrowsAsync(new KeyNotFoundException("Not found"));

        // Act
        var result = await _profileViewController.DeleteViewById(viewId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task DeleteOldViews_ReturnsOk_WhenViewsAreDeleted()
    {
        // Arrange
        var date = DateTime.Now;

        // Act
        var result = await _profileViewController.DeleteOldViews(date) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task DeleteOldViews_ReturnsBadRequest_WhenDateIsInvalid()
    {
        // Arrange
        var date = DateTime.Now;
        _profileViewServiceMock.Setup(service => service.DeleteOldViews(date)).ThrowsAsync(new InvalidDateTimeException("Invalid date"));

        // Act
        var result = await _profileViewController.DeleteOldViews(date) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }
}