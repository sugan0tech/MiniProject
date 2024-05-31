using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Membership;

public class MembershipControllerTests
{
    private Mock<IMembershipService> _membershipServiceMock;
    private Mock<ILogger<MembershipController>> _loggerMock;
    private MembershipController _membershipController;

    [SetUp]
    public void SetUp()
    {
        _membershipServiceMock = new Mock<IMembershipService>();
        _loggerMock = new Mock<ILogger<MembershipController>>();
        _membershipController = new MembershipController(_membershipServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetByProfileId_ReturnsOk_WhenMembershipExists()
    {
        // Arrange
        var profileId = 1;
        var membershipDto = new MembershipDto { MembershipId = profileId , Type = "Premium"};
        _membershipServiceMock.Setup(service => service.GetByProfileId(profileId)).ReturnsAsync(membershipDto);

        // Act
        var result = await _membershipController.GetByProfileId(profileId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(membershipDto, result.Value);
    }

    [Test]
    public async Task GetByProfileId_ReturnsNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var profileId = 1;
        _membershipServiceMock.Setup(service => service.GetByProfileId(profileId)).ThrowsAsync(new KeyNotFoundException("Membership not found"));

        // Act
        var result = await _membershipController.GetByProfileId(profileId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task GetByUserId_ReturnsOk_WhenMembershipExists()
    {
        // Arrange
        var userId = 1;
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.GetByUserId(userId)).ReturnsAsync(membershipDto);

        // Act
        var result = await _membershipController.GetByUserId(userId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(membershipDto, result.Value);
    }

    [Test]
    public async Task GetByUserId_ReturnsNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _membershipServiceMock.Setup(service => service.GetByUserId(userId)).ThrowsAsync(new KeyNotFoundException("Membership not found"));

        // Act
        var result = await _membershipController.GetByUserId(userId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task DeleteById_ReturnsOk_WhenMembershipIsDeleted()
    {
        // Arrange
        var membershipId = 1;
        var membershipDto = new MembershipDto { MembershipId = membershipId, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.DeleteById(membershipId)).ReturnsAsync(membershipDto);

        // Act
        var result = await _membershipController.DeleteById(membershipId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(membershipDto, result.Value);
    }

    [Test]
    public async Task DeleteById_ReturnsNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var membershipId = 1;
        _membershipServiceMock.Setup(service => service.DeleteById(membershipId)).ThrowsAsync(new KeyNotFoundException("Membership not found"));

        // Act
        var result = await _membershipController.DeleteById(membershipId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task Add_ReturnsOk_WhenMembershipIsAdded()
    {
        // Arrange
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.Add(membershipDto)).ReturnsAsync(membershipDto);

        // Act
        var result = await _membershipController.Add(membershipDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(membershipDto, result.Value);
    }

    [Test]
    public async Task Add_ReturnsBadRequest_WhenMembershipAlreadyExists()
    {
        // Arrange
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.Add(membershipDto)).ThrowsAsync(new AlreadyExistingEntityException("Membership already exists"));

        // Act
        var result = await _membershipController.Add(membershipDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Test]
    public async Task Update_ReturnsOk_WhenMembershipIsUpdated()
    {
        // Arrange
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.Update(membershipDto)).ReturnsAsync(membershipDto);

        // Act
        var result = await _membershipController.Update(membershipDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(membershipDto, result.Value);
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.Update(membershipDto)).ThrowsAsync(new KeyNotFoundException("Membership not found"));

        // Act
        var result = await _membershipController.Update(membershipDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task ValidateById_ReturnsOk_WhenMembershipIsValidated()
    {
        // Arrange
        var membershipId = 1;
        _membershipServiceMock.Setup(service => service.Validate(membershipId)).Returns(Task.CompletedTask);

        // Act
        var result = await _membershipController.Validate(membershipId) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task ValidateById_ReturnsNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var membershipId = 1;
        _membershipServiceMock.Setup(service => service.Validate(membershipId)).ThrowsAsync(new KeyNotFoundException("Membership not found"));

        // Act
        var result = await _membershipController.Validate(membershipId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task ValidateByDto_ReturnsOk_WhenMembershipIsValidated()
    {
        // Arrange
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.Validate(membershipDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _membershipController.Validate(membershipDto) as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }

    [Test]
    public async Task ValidateByDto_ReturnsNotFound_WhenMembershipDoesNotExist()
    {
        // Arrange
        var membershipDto = new MembershipDto { MembershipId = 1, Type = "Premium"};
        _membershipServiceMock.Setup(service => service.Validate(membershipDto)).ThrowsAsync(new KeyNotFoundException("Membership not found"));

        // Act
        var result = await _membershipController.Validate(membershipDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task ValidateAll_ReturnsOk_WhenAllMembershipsAreValidated()
    {
        // Arrange
        _membershipServiceMock.Setup(service => service.ValidateAll()).Returns(Task.CompletedTask);

        // Act
        var result = await _membershipController.ValidateAll() as OkResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }
}