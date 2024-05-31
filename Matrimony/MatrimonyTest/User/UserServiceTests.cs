using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.User;

public class UserServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.User.User>> _userRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<UserService>> _loggerMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IBaseRepo<MatrimonyApiService.User.User>>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var user = new MatrimonyApiService.User.User
        {
            Id = userId,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var userDto = new UserDto { UserId = userId };

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.GetById(userId);

        // ClassicAssert
        ClassicAssert.AreEqual(userDto, result);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetById(userId));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        // Arrange
        var user1 = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var user2 = new MatrimonyApiService.User.User
        {
            Id = 2,
            Email = "test2@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var users = new List<MatrimonyApiService.User.User> {user1, user2};
        var userDtos = new List<UserDto> { new UserDto { UserId = 1 }, new UserDto { UserId = 2 } };

        _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(users);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(It.IsAny<MatrimonyApiService.User.User>())).Returns((MatrimonyApiService.User.User src) => new UserDto { UserId = src.Id });

        // Act
        var result = await _userService.GetAll();

        // ClassicAssert
        ClassicAssert.AreEqual(userDtos.Count, result.Count);
        for (int i = 0; i < userDtos.Count; i++)
        {
            ClassicAssert.AreEqual(userDtos[i].UserId, result[i].UserId);
        }
    }

    [Test]
    public async Task Add_ShouldReturnAddedUser()
    {
        // Arrange
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var userDto = new UserDto
        {
            UserId = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };

        _userRepositoryMock.Setup(repo => repo.Add(user)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);
        _mapperMock.Setup(mapper => mapper.Map<MatrimonyApiService.User.User>(userDto)).Returns(user);

        // Act
        var result = await _userService.Add(userDto);

        // ClassicAssert
        ClassicAssert.AreEqual(userDto, result);
    }

    [Test]
    public async Task Update_ShouldReturnUpdatedUser_WhenUserExists()
    {
        // Arrange
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var userDto = new UserDto
        {
            UserId = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787"
        };

        _mapperMock.Setup(mapper => mapper.Map<MatrimonyApiService.User.User>(userDto)).Returns(user);
        _userRepositoryMock.Setup(repo => repo.Update(user)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.Update(userDto);

        // ClassicAssert
        ClassicAssert.AreEqual(userDto, result);
    }
    
    [Test]
    public async Task ValidateUser_WithStats_ShouldUpdate()
    {
        // Arrange
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var updatedUser = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            IsVerified = true,
            Password = [],
            HashKey = []
        };
        var userDto = new UserDto
        {
            UserId = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            IsVerified = true
        };

        _mapperMock.Setup(mapper => mapper.Map<MatrimonyApiService.User.User>(userDto)).Returns(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(updatedUser)).Returns(userDto);
        _userRepositoryMock.Setup(repo => repo.Update(user)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.Validate(1, true);

        // ClassicAssert
        ClassicAssert.AreEqual(true, result.IsVerified);
    }

    [Test]
    public async Task ValidateInvalidUser_WithStats_ShouldThrowKeyNotFoundException()
    {
        _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.Validate(1, true));
    }
    
    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password =
            [
            ],
            HashKey =
            [
            ]
        };
        var userDto = new UserDto { UserId = 1 };
        _mapperMock.Setup(mapper => mapper.Map<MatrimonyApiService.User.User>(userDto)).Returns(user);
        _userRepositoryMock.Setup(repo => repo.Update(user)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.Update(userDto));
    }

    [Test]
    public async Task GetByEmail_ShouldReturnUser_WhenEmailExists()
    {
        // Arrange
        var email = "test@example.com";
        // Arrange
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = email,
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var userDto = new UserDto
        {
            UserId = 1,
            Email = email,
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787"
        };

        _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.User.User> { user });
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.GetByEmail(email);

        // ClassicAssert
        ClassicAssert.AreEqual(user.Email, result.Email);
    }

    [Test]
    public void GetByEmail_ShouldThrowUserNotFoundException_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = "test@example.com";
        _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<MatrimonyApiService.User.User>());

        // Act & ClassicAssert
        Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetByEmail(email));
    }

    [Test]
    public async Task DeleteById_ShouldReturnDeletedUser()
    {
        // Arrange
        var userId = 1;
        var user = new MatrimonyApiService.User.User
        {
            Id = 1,
            Email = "Test@mail.com",
            FirstName = "test",
            LastName = "test",
            PhoneNumber = "88787",
            Password = new byte[]
            {
            },
            HashKey = new byte[]
            {
            }
        };
        var userDto = new UserDto { UserId = userId };

        _userRepositoryMock.Setup(repo => repo.DeleteById(userId)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.DeleteById(userId);

        // ClassicAssert
        ClassicAssert.AreEqual(userDto, result);
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _userRepositoryMock.Setup(repo => repo.DeleteById(userId)).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteById(userId));
    }
}