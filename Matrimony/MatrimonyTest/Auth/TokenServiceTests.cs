using MatrimonyApiService.Auth;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Auth;

public class TokenServiceTests
{
    private Mock<IConfiguration> _configurationMock;
    private TokenService _tokenService;

    [SetUp]
    public void Setup()
    {
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c.GetSection("TokenKey").GetSection("JWT").Value)
            .Returns("didididiidididididididididididididididididididdiSomeSecretKey");
        _tokenService = new TokenService(_configurationMock.Object);
    }

    [Test]
    public void GenerateToken_ValidUser_ReturnsToken()
    {
        var user = new UserDto
        {
            UserId = 1,
            Email = "user@example.com",
            Role = Role.User.ToString(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = "idi"u8.ToArray(),
            HashKey = "sid"u8.ToArray()
        };
        var token = _tokenService.GenerateToken(user);

        ClassicAssert.IsNotNull(token);
        ClassicAssert.IsInstanceOf<string>(token);
    }

    [Test]
    public void GetPayload_ValidToken_ReturnsPayloadDto()
    {
        var user = new UserDto
        {
            UserId = 1,
            Email = "user@example.com",
            Role = Role.User.ToString(),
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "1234567890",
            Password = "idi"u8.ToArray(),
            HashKey = "sid"u8.ToArray()
        };
        var token = _tokenService.GenerateToken(user);
        var payload = _tokenService.GetPayload(token);

        ClassicAssert.IsNotNull(payload);
        ClassicAssert.AreEqual(user.UserId, payload.Id);
        ClassicAssert.AreEqual(user.Email, payload.Email);
    }

    [Test]
    public void TokenService_NoSecretKey_ThrowsException()
    {
        _configurationMock.Setup(c => c.GetSection("TokenKey").GetSection("JWT").Value).Returns<string>(null);
        Assert.Throws<NoSecretKeyFoundException>(() => new TokenService(_configurationMock.Object));
    }
}