using System.Security.Claims;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Report;

[TestFixture]
public class ReportControllerTests
{
    private Mock<IBaseService<MatrimonyApiService.Report.Report, ReportDto>> _mockReportService;
    private Mock<ILogger<ReportController>> _mockLogger;
    private ReportController _reportController;
    private List<Claim> _claims;

    [SetUp]
    public void Setup()
    {
        _mockReportService = new Mock<IBaseService<MatrimonyApiService.Report.Report, ReportDto>>();
        _mockLogger = new Mock<ILogger<ReportController>>();
        _reportController = new ReportController(_mockReportService.Object, _mockLogger.Object);
        
        _claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };
        var identity = new ClaimsIdentity(_claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _reportController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Test]
    public async Task Get_ShouldReturnOk_WhenReportExists()
    {
        // Arrange
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        _mockReportService.Setup(service => service.GetById(It.IsAny<int>())).ReturnsAsync(reportDto);

        // Act
        var result = await _reportController.Get(1);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        ClassicAssert.AreEqual(reportDto, okResult.Value);
    }

    [Test]
    public async Task Get_ShouldReturnNotFound_WhenReportDoesNotExist()
    {
        // Arrange
        _mockReportService.Setup(service => service.GetById(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _reportController.Get(99);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        ClassicAssert.IsInstanceOf<ErrorModel>(notFoundResult.Value);
    }

    [Test]
    public async Task GetAll_ShouldReturnOk_WhenReportsExist()
    {
        // Arrange
        var reports = new List<ReportDto>
        {
            new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now },
            new ReportDto { ProfileId = 2, ReportedById = 2, ReportedAt = DateTime.Now }
        };
        _mockReportService.Setup(service => service.GetAll()).ReturnsAsync(reports);

        // Act
        var result = await _reportController.GetAll();

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        ClassicAssert.AreEqual(reports, okResult.Value);
    }

    [Test]
    public async Task Add_ShouldReturnOk_WhenReportIsAdded()
    {
        // Arrange
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        _mockReportService.Setup(service => service.Add(It.IsAny<ReportDto>())).ReturnsAsync(reportDto);

        // Act
        var result = await _reportController.Add(reportDto);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        ClassicAssert.AreEqual(reportDto, okResult.Value);
    }

       [Test]
    public async Task Add_ShouldReturnBadRequest_WhenArgumentNullExceptionOccurs()
    {
        // Arrange
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        var exception = new ArgumentNullException("Report cannot be null");
        _mockReportService.Setup(service => service.Add(It.IsAny<ReportDto>())).ThrowsAsync(exception);

        // Act
        var result = await _reportController.Add(reportDto);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        ClassicAssert.IsInstanceOf<ErrorModel>(badRequestResult.Value);
        var errorModel = badRequestResult.Value as ErrorModel;
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, errorModel.Status);
        ClassicAssert.AreEqual(exception.Message, errorModel.Message);
    }

    [Test]
    public async Task Add_ShouldReturnForbidden_WhenAuthenticationExceptionOccurs()
    {
        // Arrange
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        var exception = new AuthenticationException("User is not authorized");
        _mockReportService.Setup(service => service.Add(It.IsAny<ReportDto>())).ThrowsAsync(exception);

        // Act
        var result = await _reportController.Add(reportDto);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<ObjectResult>(result);
        var forbiddenResult = result as ObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, forbiddenResult.StatusCode);
        ClassicAssert.IsInstanceOf<ErrorModel>(forbiddenResult.Value);
        var errorModel = forbiddenResult.Value as ErrorModel;
        ClassicAssert.AreEqual(StatusCodes.Status403Forbidden, errorModel.Status);
        ClassicAssert.AreEqual(exception.Message, errorModel.Message);
    }
    
    [Test]
    public async Task DeleteById_ShouldReturnOk_WhenReportIsDeleted()
    {
        // Arrange
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        _mockReportService.Setup(service => service.DeleteById(It.IsAny<int>())).ReturnsAsync(reportDto);

        // Act
        var result = await _reportController.DeleteById(1);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        ClassicAssert.AreEqual(reportDto, okResult.Value);
    }

    [Test]
    public async Task DeleteById_ShouldReturnNotFound_WhenReportDoesNotExist()
    {
        // Arrange
        _mockReportService.Setup(service => service.DeleteById(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _reportController.DeleteById(99);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        ClassicAssert.IsInstanceOf<ErrorModel>(notFoundResult.Value);
    }
}