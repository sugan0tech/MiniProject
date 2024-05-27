using MatrimonyApiService.Auth;
using MatrimonyApiService.Exceptions;
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
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
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
        var token = _tokenService.GenerateToken(user);

        ClassicAssert.IsNotNull(token);
        ClassicAssert.IsInstanceOf<string>(token);
    }

    [Test]
    public void GetPayload_ValidToken_ReturnsPayloadDto()
    {
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
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
        var token = _tokenService.GenerateToken(user);
        var payload = _tokenService.GetPayload(token);

        ClassicAssert.IsNotNull(payload);
        ClassicAssert.AreEqual(user.Id, payload.Id);
        ClassicAssert.AreEqual(user.Email, payload.Email);
    }

    [Test]
    public void TokenService_NoSecretKey_ThrowsException()
    {
        _configurationMock.Setup(c => c.GetSection("TokenKey").GetSection("JWT").Value).Returns<string>(null);
        Assert.Throws<NoSecretKeyFoundException>(() => new TokenService(_configurationMock.Object));
    }
}