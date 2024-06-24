using System.Security.Claims;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.User;

public class UserControllerTests
{
    private Mock<IUserService> _mockUserService;
    private Mock<ILogger<UserController>> _mockLogger;
    private UserController _controller;
    private List<Claim> _claims;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<UserController>>();
        _controller = new UserController(_mockUserService.Object, _mockLogger.Object);

        _claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };
        var identity = new ClaimsIdentity(_claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Test]
    public async Task GetAll_ShouldReturnOkWithUsers()
    {
        // Arrange
        var users = new List<UserDto> { new UserDto { UserId = 1, Email = "test@test.com" } };
        _mockUserService.Setup(service => service.GetAll()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll();

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(users, okResult.Value);
    }

    [Test]
    public async Task Add_ShouldReturnCreatedWithUser()
    {
        // Arrange
        var user = new UserDto { Email = "test@test.com" };
        _mockUserService.Setup(service => service.Add(It.IsAny<UserDto>())).ReturnsAsync(user);

        // Act
        var result = await _controller.Add(user);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        ClassicAssert.AreEqual(201, objectResult.StatusCode);
        ClassicAssert.AreEqual(user, objectResult.Value);
    }

    [Test]
    public async Task Update_ShouldReturnOkWithUser()
    {
        // Arrange
        var user = new UserDto { Email = "test@test.com" };
        _mockUserService.Setup(service => service.Update(It.IsAny<UserDto>())).ReturnsAsync(user);

        // Act
        var result = await _controller.Update(user);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(user, okResult.Value);
    }

    [Test]
    public async Task Update_ShouldReturnNotFoundWhenUserNotFound()
    {
        // Arrange
        var user = new UserDto { Email = "test@test.com" };
        _mockUserService.Setup(service => service.Update(It.IsAny<UserDto>()))
            .Throws(new KeyNotFoundException("User not found"));

        // Act
        var result = await _controller.Update(user);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task GetByEmail_ShouldReturnOkWithUser()
    {
        // Arrange
        var user = new UserDto { Email = "test@test.com" };
        _mockUserService.Setup(service => service.GetByEmail(It.IsAny<string>())).ReturnsAsync(user);

        // Act
        var result = await _controller.GetByEmail(user.Email);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(user, okResult.Value);
    }

    [Test]
    public async Task GetByEmail_ShouldReturnNotFoundWhenUserNotFound()
    {
        // Arrange
        _mockUserService.Setup(service => service.GetByEmail(It.IsAny<string>()))
            .Throws(new UserNotFoundException("User not found"));

        // Act
        var result = await _controller.GetByEmail("nonexistent@test.com");

        // ClassicAssert
        ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task DeleteById_ShouldReturnOkWithUser()
    {
        // Arrange
        var user = new UserDto { UserId = 1, Email = "test@test.com" };
        _mockUserService.Setup(service => service.DeleteById(It.IsAny<int>())).ReturnsAsync(user);

        // Act
        var result = await _controller.DeleteById(1);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(user, okResult.Value);
    }

    [Test]
    public async Task DeleteById_ShouldReturnNotFoundWhenUserInvalid()
    {
        // Act
        var result = await _controller.DeleteById(999);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<ObjectResult>(result);
        var forbidden = result as ObjectResult;
        ClassicAssert.AreEqual(403, forbidden.StatusCode);
    }

    [Test]
    public async Task DeleteById_ShouldReturnNotFoundWhenUserNotFoundWithToken()
    {
        // Arrange
        _claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(_claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
        _mockUserService.Setup(service => service.DeleteById(It.IsAny<int>()))
            .Throws(new KeyNotFoundException("User not found"));

        // Act
        var result = await _controller.DeleteById(999);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
    }

    [Test]
    public async Task Validate_ShouldReturnOkWithUser()
    {
        // Arrange
        var user = new UserDto { UserId = 1, Email = "test@test.com" };
        _mockUserService.Setup(service => service.Validate(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(user);

        // Act
        var result = await _controller.Validate(1, true);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(user, okResult.Value);
    }

    [Test]
    public async Task Validate_ShouldReturnNotFoundWhenUserNotFound()
    {
        // Arrange
        _mockUserService.Setup(service => service.Validate(It.IsAny<int>(), It.IsAny<bool>()))
            .Throws(new KeyNotFoundException("User not found"));

        // Act
        var result = await _controller.Validate(999, true);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        ClassicAssert.AreEqual(404, notFoundResult.StatusCode);
    }
}