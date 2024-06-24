using MatrimonyApiService.Auth;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Auth;

public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock;
    private Mock<ILogger<AuthController>> _loggerMock;
    private AuthController _authController;

    [SetUp]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthService>();
        _loggerMock = new Mock<ILogger<AuthController>>();
        _authController = new AuthController(_authServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "password123" };
        var authReturnDto = new AuthReturnDto { Token = "token" };
        _authServiceMock.Setup(service => service.Login(loginDto)).ReturnsAsync(authReturnDto);

        // Act
        var result = await _authController.Login(loginDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(authReturnDto, result.Value);
    }

    [Test]
    public async Task Login_ReturnsUnauthorized_WhenUserNotVerified()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "password123" };
        _authServiceMock.Setup(service => service.Login(loginDto))
            .ThrowsAsync(new UserNotVerifiedException("User not verified"));

        // Act
        var result = await _authController.Login(loginDto) as UnauthorizedObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
    }


    [Test]
    public async Task Login_ReturnsUnauthorized_CredentialsAreWrong()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "password123" };
        _authServiceMock.Setup(service => service.Login(loginDto))
            .ThrowsAsync(new AuthenticationException("User not verified"));

        // Act
        var result = await _authController.Login(loginDto) as UnauthorizedObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Test]
    public async Task Login_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        var loginDto = new LoginDTO { Email = "test@example.com", Password = "password123" };
        _authServiceMock.Setup(service => service.Login(loginDto))
            .ThrowsAsync(new UserNotFoundException("User not found"));

        // Act
        var result = await _authController.Login(loginDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerDto = new RegisterDTO
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            AddressId = 1,
            Password = "password123"
        };
        _authServiceMock.Setup(service => service.Register(registerDto)).ReturnsAsync(true);

        // Act
        var result = await _authController.Register(registerDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task Register_ReturnsUnauthorized_WhenUserNotVerified()
    {
        // Arrange
        var registerDto = new RegisterDTO
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            AddressId = 1,
            Password = "password123"
        };
        _authServiceMock.Setup(service => service.Register(registerDto))
            .ThrowsAsync(new UserNotVerifiedException("User not verified"));

        // Act
        var result = await _authController.Register(registerDto) as UnauthorizedObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Test]
    public async Task Register_ReturnsUnauthorized_WhenUserWithWrongCreds()
    {
        // Arrange
        var registerDto = new RegisterDTO
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            AddressId = 1,
            Password = "password123"
        };
        _authServiceMock.Setup(service => service.Register(registerDto))
            .ThrowsAsync(new AuthenticationException("User not verified"));

        // Act
        var result = await _authController.Register(registerDto) as UnauthorizedObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Test]
    public async Task Register_ReturnsBadRequest_WhenRegisteringWithSameEmail()
    {
        // Arrange
        var registerDto = new RegisterDTO
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            AddressId = 1,
            Password = "password123"
        };
        _authServiceMock.Setup(service => service.Register(registerDto)).ThrowsAsync(new DbUpdateException());

        // Act
        var result = await _authController.Register(registerDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task ResetPassword_ReturnsOk_WhenResetIsSuccessful()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@example.com",
            Password = "oldPassword123",
            NewPassword = "newPassword123"
        };
        var returnDto = new AuthReturnDto { Token = "did" };
        _authServiceMock.Setup(service => service.ResetPassword(resetPasswordDto)).ReturnsAsync(returnDto);

        // Act
        var result = await _authController.ResetPassword(resetPasswordDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(returnDto, result.Value);
    }

    [Test]
    public async Task ResetPassword_ReturnsUnauthorized_WhenUserNotVerified()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@example.com",
            Password = "oldPassword123",
            NewPassword = "newPassword123"
        };
        _authServiceMock.Setup(service => service.ResetPassword(resetPasswordDto))
            .ThrowsAsync(new UserNotVerifiedException("User not verified"));

        // Act
        var result = await _authController.ResetPassword(resetPasswordDto) as UnauthorizedObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Test]
    public async Task ResetPassword_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@example.com",
            Password = "oldPassword123",
            NewPassword = "newPassword123"
        };
        _authServiceMock.Setup(service => service.ResetPassword(resetPasswordDto))
            .ThrowsAsync(new UserNotFoundException("User not found"));

        // Act
        var result = await _authController.ResetPassword(resetPasswordDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task ResetPassword_ReturnsUnauthorized_WhenUserWrongPassword()
    {
        // Arrange
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@example.com",
            Password = "oldPassword123",
            NewPassword = "newPassword123"
        };
        _authServiceMock.Setup(service => service.ResetPassword(resetPasswordDto))
            .ThrowsAsync(new AuthenticationException("Wrong password or email"));

        // Act
        var result = await _authController.ResetPassword(resetPasswordDto) as UnauthorizedObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status401Unauthorized, result.StatusCode);
    }
}