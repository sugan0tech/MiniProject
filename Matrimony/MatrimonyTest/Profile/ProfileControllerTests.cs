using System.Security.Claims;
using MatrimonyApiService.Profile;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using ProfileDto = MatrimonyApiService.Profile.ProfileDto;

namespace MatrimonyTest.Profile;

public class ProfileControllerTests
{
    private Mock<IProfileService> _profileServiceMock;
    private Mock<ILogger<ProfileController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private ProfileController _profileController;
    private List<Claim> _claims;

    [SetUp]
    public void SetUp()
    {
        _profileServiceMock = new Mock<IProfileService>();
        _loggerMock = new Mock<ILogger<ProfileController>>();
        _mediatorMock = new Mock<IMediator>();
        _profileController = new ProfileController(_profileServiceMock.Object, _mediatorMock.Object, _loggerMock.Object);
        
        _claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };
        var identity = new ClaimsIdentity(_claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _profileController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Test]
    public async Task GetProfilePreviewForManager_ReturnsOk_WhenProfilesExist()
    {
        // Arrange
        var managerId = 1;
        var profiles = new List<ProfilePreviewDto> { new ProfilePreviewDto { ProfileId = 1, Education = "School", Ethnicity = "Indian", MotherTongue = "Tamil", Religion = "Hindu", Occupation = "Doctor"} };
        _profileServiceMock.Setup(service => service.GetProfilesByManager(managerId)).ReturnsAsync(profiles);

        // Act
        var result = await _profileController.GetProfilePreviewForManager(managerId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profiles, result.Value);
    }

    [Test]
    public async Task GetProfilePreviewForManager_ReturnsNotFound_WhenProfilesDoNotExist()
    {
        // Arrange
        var managerId = 1;
        _profileServiceMock.Setup(service => service.GetProfilesByManager(managerId)).ThrowsAsync(new KeyNotFoundException("Manager not found"));

        // Act
        var result = await _profileController.GetProfilePreviewForManager(managerId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetProfilePreviewForManager_ReturnsForbidden_WhenProfilesDoNotExistforManager()
    {
        // Arrange
        var managerId = 2;

        // Act
        var result = await _profileController.GetProfilePreviewForManager(managerId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetProfilePreviewById_ReturnsOk_WhenProfilePreviewExists()
    {
        // Arrange
        var profileId = 1;
        var profilePreviewDto = new ProfilePreviewDto { ProfileId = profileId };
        _profileServiceMock.Setup(service => service.GetProfilePreviewById(profileId)).ReturnsAsync(profilePreviewDto);

        // Act
        var result = await _profileController.GetProfilePreviewById(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profilePreviewDto, result.Value);
    }

    [Test]
    public async Task GetProfilePreviewById_ReturnsNotFound_WhenProfilePreviewDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfilePreviewById(profileId)).ThrowsAsync(new KeyNotFoundException("Profile preview not found"));

        // Act
        var result = await _profileController.GetProfilePreviewById(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    // Existing test cases...

    [Test]
    public async Task GetProfileById_ReturnsOk_WhenProfileExists()
    {
        // Arrange
        var profileId = 1;
        var profileDto = new ProfileDto { ProfileId = profileId };
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.GetProfileById(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task GetProfileById_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.GetProfileById(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetProfileByUserId_ReturnsOk_WhenProfileExists()
    {
        // Arrange
        var userId = 1;
        var profileDto = new ProfileDto { UserId = userId };
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.GetProfileByUserId(userId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task GetProfileByUserId_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _profileServiceMock.Setup(service => service.GetProfileByUserId(userId)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.GetProfileByUserId(userId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetProfileByUserId_ReturnsForbidden_WhenProfileAttachedToDifferentUser()
    {
        // Arrange
        var userId = 2;

        // Act
        var result = await _profileController.GetProfileByUserId(userId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task AddProfile_ReturnsCreated_WhenProfileIsAdded()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1, ManagedById = 1};
        _profileServiceMock.Setup(service => service.AddProfile(profileDto)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.AddProfile(profileDto) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task AddProfile_ReturnsBadRequest_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1, ManagedById = 1};
        _profileServiceMock.Setup(service => service.AddProfile(profileDto)).ThrowsAsync(new DbUpdateException("Database error"));

        // Act
        var result = await _profileController.AddProfile(profileDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task AddProfile_ReturnsForbidden_WhenDidforWrongManager()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1, ManagedById = 2};

        // Act
        var result = await _profileController.AddProfile(profileDto) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task UpdateProfile_ReturnsOk_WhenProfileIsUpdated()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1 , ManagedById = 1};
        _profileServiceMock.Setup(service => service.UpdateProfile(profileDto)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.UpdateProfile(profileDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task UpdateProfile_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1, ManagedById = 1};
        _profileServiceMock.Setup(service => service.UpdateProfile(profileDto)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.UpdateProfile(profileDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task UpdateProfile_ReturnsForBidden_WhenManagerNotExist()
    {
        // Arrange
        var profileDto = new ProfileDto { ProfileId = 1, ManagedById = 2};

        // Act
        var result = await _profileController.UpdateProfile(profileDto) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task DeleteProfileById_ReturnsOk_WhenProfileIsDeleted()
    {
        // Arrange
        var profileId = 1;
        var profileDto = new ProfileDto { ProfileId = profileId };
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(new ProfileDto{ManagedById = 1});
        _profileServiceMock.Setup(service => service.DeleteProfileById(profileId)).ReturnsAsync(profileDto);

        // Act
        var result = await _profileController.DeleteProfileById(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profileDto, result.Value);
    }

    [Test]
    public async Task DeleteProfileById_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(new ProfileDto{ManagedById = 1});
        _profileServiceMock.Setup(service => service.DeleteProfileById(profileId)).ThrowsAsync(new KeyNotFoundException("Profile not found"));

        // Act
        var result = await _profileController.DeleteProfileById(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task DeleteProfileById_ReturnsForbidden_WhenManagerDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfileById(profileId)).ReturnsAsync(new ProfileDto{ManagedById = 2});

        // Act
        var result = await _profileController.DeleteProfileById(profileId) as ObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, result.StatusCode);
    }

    [Test]
    public async Task GetMatches_ReturnsOk_WhenMatchesExist()
    {
        // Arrange
        var profileId = 1;
        var matches = new List<ProfilePreviewDto> { new ProfilePreviewDto { ProfileId = 1 } };
        _profileServiceMock.Setup(service => service.GetProfileMatches(profileId)).ReturnsAsync(matches);

        // Act
        var result = await _profileController.GetMatches(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(matches, result.Value);
    }

    [Test]
    public async Task GetMatches_ReturnsNotFound_WhenNoMatchesExist()
    {
        // Arrange
        var profileId = 1;
        _profileServiceMock.Setup(service => service.GetProfileMatches(profileId)).ThrowsAsync(new KeyNotFoundException("No matches found"));

        // Act
        var result = await _profileController.GetMatches(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetAll_ReturnsOk_WhenProfilesExist()
    {
        // Arrange
        var profiles = new List<ProfileDto> { new ProfileDto { ProfileId = 1 } };
        _profileServiceMock.Setup(service => service.GetAll()).ReturnsAsync(profiles);

        // Act
        var result = await _profileController.GetAll() as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(profiles, result.Value);
    }
}