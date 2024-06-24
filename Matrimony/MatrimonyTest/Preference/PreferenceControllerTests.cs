using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Preference;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Preference;

public class PreferenceControllerTests
{
    private Mock<IPreferenceService> _preferenceServiceMock;
    private Mock<ILogger<PreferenceController>> _loggerMock;
    private PreferenceController _preferenceController;

    [SetUp]
    public void SetUp()
    {
        _preferenceServiceMock = new Mock<IPreferenceService>();
        _loggerMock = new Mock<ILogger<PreferenceController>>();
        _preferenceController = new PreferenceController(_preferenceServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Add_ReturnsOk_WhenPreferenceIsAdded()
    {
        // Arrange
        var preferenceDto = new PreferenceDto { PreferenceId = 1 };
        _preferenceServiceMock.Setup(service => service.Add(preferenceDto)).ReturnsAsync(preferenceDto);

        // Act
        var result = await _preferenceController.Add(preferenceDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(preferenceDto, result.Value);
    }

    [Test]
    public async Task Add_ReturnsBadRequest_WhenValidationExceptionOccurs()
    {
        // Arrange
        var preferenceDto = new PreferenceDto { PreferenceId = 1 };
        var validationResult = new ValidationResult("Validation error");
        _preferenceServiceMock.Setup(service => service.Add(preferenceDto))
            .ThrowsAsync(new ValidationException(validationResult.ToString()));

        // Act
        var result = await _preferenceController.Add(preferenceDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        ClassicAssert.AreEqual(validationResult.ToString(), result.Value);
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenPreferenceExists()
    {
        // Arrange
        var preferenceId = 1;
        var preferenceDto = new PreferenceDto { PreferenceId = preferenceId };
        _preferenceServiceMock.Setup(service => service.GetById(preferenceId)).ReturnsAsync(preferenceDto);

        // Act
        var result = await _preferenceController.GetById(preferenceId) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(preferenceDto, result.Value);
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenPreferenceDoesNotExist()
    {
        // Arrange
        var preferenceId = 1;
        _preferenceServiceMock.Setup(service => service.GetById(preferenceId))
            .ThrowsAsync(new KeyNotFoundException("Preference not found"));

        // Act
        var result = await _preferenceController.GetById(preferenceId) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task Update_ReturnsOk_WhenPreferenceIsUpdated()
    {
        // Arrange
        var preferenceDto = new PreferenceDto { PreferenceId = 1 };
        _preferenceServiceMock.Setup(service => service.Update(preferenceDto)).ReturnsAsync(preferenceDto);

        // Act
        var result = await _preferenceController.Update(preferenceDto) as OkObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        ClassicAssert.AreEqual(preferenceDto, result.Value);
    }

    [Test]
    public async Task Update_ReturnsNotFound_WhenPreferenceDoesNotExist()
    {
        // Arrange
        var preferenceDto = new PreferenceDto { PreferenceId = 1 };
        _preferenceServiceMock.Setup(service => service.Update(preferenceDto))
            .ThrowsAsync(new KeyNotFoundException("Preference not found"));

        // Act
        var result = await _preferenceController.Update(preferenceDto) as NotFoundObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_WhenValidationExceptionOccurs()
    {
        // Arrange
        var preferenceDto = new PreferenceDto { PreferenceId = 1 };
        var validationResult = new ValidationResult("Validation error");
        _preferenceServiceMock.Setup(service => service.Update(preferenceDto))
            .ThrowsAsync(new ValidationException(validationResult.ToString()));

        // Act
        var result = await _preferenceController.Update(preferenceDto) as BadRequestObjectResult;

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        ClassicAssert.AreEqual(validationResult.ToString(), result.Value);
    }
}