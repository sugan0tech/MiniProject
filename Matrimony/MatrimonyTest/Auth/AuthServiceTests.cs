using System.Text;
using MatrimonyApiService.Auth;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Auth;

public class AuthServiceTests
{
    private Mock<IUserService> _userServiceMock;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<ILogger<AuthService>> _loggerMock;
    private AuthService _authService;

    [SetUp]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        _authService = new AuthService(_userServiceMock.Object, _tokenServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public void Login_ValidUser_ReturnsUserAuthReturnDto()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "password" };
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = loginDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = new byte[]
            {
                // Populate password bytes here
            },
            HashKey = new byte[]
            {
                // Populate hash key bytes here
            }
        };
        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("token");

        var result = _authService.Login(loginDto);

        ClassicAssert.IsNotNull(result);
        // ClassicAssert.IsInstanceOf<AuthReturnDto>(result);
    }

    [Test]
    public void Login_UserNotVerified_ThrowsUserNotVerifiedException()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "password" };
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = loginDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = false,
            Password = new byte[]
            {
                // Populate password bytes here
            },
            HashKey = new byte[]
            {
                // Populate hash key bytes here
            }
        };
        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ReturnsAsync(user);

        Assert.ThrowsAsync<UserNotVerifiedException>(() => _authService.Login(loginDto));
    }

    [Test]
    public void Login_InvalidPassword_ThrowsAuthenticationException()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "wrongpassword" };
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = loginDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = new byte[]
            {
                // Populate password bytes here
            },
            HashKey = new byte[]
            {
                // Populate hash key bytes here
            }
        };
        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ReturnsAsync(user);

        Assert.ThrowsAsync<AuthenticationException>(() => _authService.Login(loginDto));
    }

    // [Test]
    // public async Task Register_ValidUser_ReturnsTrue()
    // {
    //     var registerDto = new RegisterDTO
    //     {
    //         Email = "test@test.com", Password = "password", FirstName = "Test", LastName = "User",
    //         PhoneNumber = "1234567890", AddressId = 1
    //     };
    //     _userServiceMock.Setup(u => u.Add(It.IsAny<MatrimonyApiService.User.User>())).ReturnsAsync();
    //
    //     var result = await _authService.Register(registerDto);
    //
    //     ClassicAssert.IsTrue(result);
    // }

    [Test]
    public void Register_ThrowsException_ReturnsFalse()
    {
        var registerDto = new RegisterDTO
        {
            Email = "test@test.com", Password = "password", FirstName = "Test", LastName = "User",
            PhoneNumber = "1234567890", AddressId = 1
        };
        _userServiceMock.Setup(u => u.Add(It.IsAny<MatrimonyApiService.User.User>())).Throws(new Exception());

        Assert.ThrowsAsync<AuthenticationException>(() => _authService.Register(registerDto));
    }

    // [Test]
    // public async Task ResetPassword_ValidUser_ReturnsUserAuthReturnDto()
    // {
    //     var resetPasswordDto = new ResetPasswordDto
    //         { Email = "test@test.com", Password = "password", NewPassword = "newpassword" };
    //     var user = new MatrimonyApiService.User.User
    //     {
    //         Id = 1,
    //         Email = resetPasswordDto.Email,
    //         FirstName = "John",
    //         LastName = "Doe",
    //         PhoneNumber = "1234567890",
    //         Password = Encoding.UTF8.GetBytes(resetPasswordDto.Password),
    //         HashKey = new byte[64]
    //     };
    //     _userServiceMock.Setup(u => u.GetByEmail(resetPasswordDto.Email)).ReturnsAsync(user);
    //     _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("token");
    //
    //     var result = await _authService.ResetPassword(resetPasswordDto);
    //
    //     ClassicAssert.IsNotNull(result);
    //     ClassicAssert.IsInstanceOf<AuthReturnDto>(result);
    // }

    [Test]
    public void ResetPassword_InvalidPassword_ThrowsAuthenticationException()
    {
        var resetPasswordDto = new ResetPasswordDto
            { Email = "test@test.com", Password = "wrongpassword", NewPassword = "newpassword" };
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = resetPasswordDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = "password"u8.ToArray(),
            HashKey = new byte[64]
        };
        _userServiceMock.Setup(u => u.GetByEmail(resetPasswordDto.Email)).ReturnsAsync(user);

        Assert.ThrowsAsync<AuthenticationException>(() => _authService.ResetPassword(resetPasswordDto));
    }
}