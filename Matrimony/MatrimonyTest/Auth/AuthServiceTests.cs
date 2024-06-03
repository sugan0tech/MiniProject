using MatrimonyApiService.Auth;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.EntityFrameworkCore;
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
    public async Task Login_ValidUser_ReturnsAuthReturnDto()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "password" };
        var user = new UserDto
        {
            UserId = 1,
            Email = loginDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = [186,56,47,235,101,60,176,103,70,230,93,19,32,157,184,7,191,190,51,203,203,4,116,50,193,19,11,65,201,86,163,131,61,108,146,126,65,196,65,72,168,44,39,177,251,195,42,53,207,131,236,52,125,241,207,168,187,89,4,106,87,134,149,224],
            HashKey = []
        };

        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("token");

        var result = await _authService.Login(loginDto);

        ClassicAssert.IsNotNull(result);
        ClassicAssert.IsInstanceOf<AuthReturnDto>(result);
        ClassicAssert.AreEqual("token", result.Token);
    }

    [Test]
    public void Login_UserNotVerified_ThrowsUserNotVerifiedException()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "password" };
        var user = new UserDto
        {
            UserId = 1,
            Email = loginDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = false,
            Password = Convert.FromBase64String("QWxhZGRpbjpPcGVuU2VzYW1l"), // example hashed password
            HashKey = Convert.FromBase64String("Q2hhbGxlbmdlQWNjZXB0ZWQ=") // example key
        };

        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ReturnsAsync(user);

        Assert.ThrowsAsync<UserNotVerifiedException>(() => _authService.Login(loginDto));
    }

    [Test]
    public void Login_InvalidUser_ThrowsUserNotFoundException()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "password" };

        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ThrowsAsync(new UserNotFoundException("msg"));

        Assert.ThrowsAsync<UserNotFoundException>(() => _authService.Login(loginDto));
    }
    [Test]
    public void Login_InvalidPassword_ThrowsAuthenticationException()
    {
        var loginDto = new LoginDTO { Email = "test@test.com", Password = "wrongpassword" };
        var user = new UserDto
        {
            UserId = 1,
            Email = loginDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            IsVerified = true,
            Password = Convert.FromBase64String("QWxhZGRpbjpPcGVuU2VzYW1l"), // example hashed password
            HashKey = Convert.FromBase64String("Q2hhbGxlbmdlQWNjZXB0ZWQ=") // example key
        };

        _userServiceMock.Setup(u => u.GetByEmail(loginDto.Email)).ReturnsAsync(user);

        Assert.ThrowsAsync<MatrimonyApiService.Exceptions.AuthenticationException>(() => _authService.Login(loginDto));
    }

    [Test]
    public async Task Register_ValidUser_ReturnsTrue()
    {
        var registerDto = new RegisterDTO
        {
            Email = "test@test.com",
            Password = "password",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            AddressId = 1
        };

        var res = new UserDto
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            AddressId = 1
        };

        _userServiceMock.Setup(u => u.Add(It.IsAny<UserDto>())).ReturnsAsync(res);

        var result = await _authService.Register(registerDto);

        ClassicAssert.IsTrue(result);
        _userServiceMock.Verify(u => u.Add(It.IsAny<UserDto>()), Times.Once);
    }

    [Test]
    public void Register_ThrowsException_ThrowsAuthenticationException()
    {
        var registerDto = new RegisterDTO
        {
            Email = "test@test.com",
            Password = "password",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            AddressId = 1
        };

        _userServiceMock.Setup(u => u.Add(It.IsAny<UserDto>())).Throws(new Exception());

        Assert.ThrowsAsync<MatrimonyApiService.Exceptions.AuthenticationException>(() => _authService.Register(registerDto));
    }

    [Test]
    public void Register_ThrowsException_ThrowsDbUpdateExceptionForNonUniqueValue()
    {
        var registerDto = new RegisterDTO
        {
            Email = "test@test.com",
            Password = "password",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            AddressId = 1
        };

        _userServiceMock.Setup(u => u.Add(It.IsAny<UserDto>())).Throws(new DbUpdateException());

        Assert.ThrowsAsync<DbUpdateException>(() => _authService.Register(registerDto));
    }

    [Test]
    public async Task ResetPassword_ValidUser_ReturnsAuthReturnDto()
    {
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@test.com",
            Password = "password",
            NewPassword = "newpassword"
        };

        var user = new UserDto
        {
            UserId = 1,
            Email = resetPasswordDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = [186,56,47,235,101,60,176,103,70,230,93,19,32,157,184,7,191,190,51,203,203,4,116,50,193,19,11,65,201,86,163,131,61,108,146,126,65,196,65,72,168,44,39,177,251,195,42,53,207,131,236,52,125,241,207,168,187,89,4,106,87,134,149,224],
            HashKey = []
        };

        _userServiceMock.Setup(u => u.GetByEmail(resetPasswordDto.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("token");

        var result = await _authService.ResetPassword(resetPasswordDto);

        ClassicAssert.IsNotNull(result);
        ClassicAssert.IsInstanceOf<AuthReturnDto>(result);
        ClassicAssert.AreEqual("token", result.Token);
        _userServiceMock.Verify(u => u.Update(It.IsAny<UserDto>()), Times.Once);
    }
    
    [Test]
    public void ResetPassword_ValidCreds_ThrowsAuthenticationException()
    {
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@test.com",
            Password = "password",
            NewPassword = "newpassword"
        };

        var user = new UserDto
        {
            UserId = 1,
            Email = resetPasswordDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = [186,56,47,235,101,60,176,103,70,230,93,19,32,157,181,7,191,190,51,203,203,4,116,50,193,19,11,65,201,86,163,131,61,108,146,126,65,196,65,72,168,44,39,177,251,195,42,53,207,131,236,52,125,241,207,168,187,89,4,106,87,134,149,224],
            HashKey = []
        };

        _userServiceMock.Setup(u => u.GetByEmail(resetPasswordDto.Email)).ReturnsAsync(user);
        _tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns("token");

        Assert.ThrowsAsync<AuthenticationException>(() => _authService.ResetPassword(resetPasswordDto));
    }

    [Test]
    public void ResetPassword_InvalidPassword_ThrowsAuthenticationException()
    {
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@test.com",
            Password = "wrongpassword",
            NewPassword = "newpassword"
        };

        var user = new UserDto
        {
            UserId = 1,
            Email = resetPasswordDto.Email,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = Convert.FromBase64String("QWxhZGRpbjpPcGVuU2VzYW1l"), // example hashed password
            HashKey = Convert.FromBase64String("Q2hhbGxlbmdlQWNjZXB0ZWQ=") // example key
        };

        _userServiceMock.Setup(u => u.GetByEmail(resetPasswordDto.Email)).ReturnsAsync(user);

        Assert.ThrowsAsync<AuthenticationException>(() => _authService.ResetPassword(resetPasswordDto));
    }

    [Test]
    public void ResetPassword_InvalidUser_ThrowsUserNotFound()
    {
        var resetPasswordDto = new ResetPasswordDto
        {
            Email = "test@test.com",
            Password = "wrongpassword",
            NewPassword = "newpassword"
        };

        _userServiceMock.Setup(u => u.GetByEmail(resetPasswordDto.Email)).ThrowsAsync(new UserNotFoundException("msg"));

        Assert.ThrowsAsync<UserNotFoundException>(() => _authService.ResetPassword(resetPasswordDto));
    }
}