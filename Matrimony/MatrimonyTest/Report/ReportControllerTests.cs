using MatrimonyApiService.Commons;
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

    [SetUp]
    public void Setup()
    {
        _mockReportService = new Mock<IBaseService<MatrimonyApiService.Report.Report, ReportDto>>();
        _mockLogger = new Mock<ILogger<ReportController>>();
        _reportController = new ReportController(_mockReportService.Object, _mockLogger.Object);
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
    public async Task Add_ShouldReturnBadRequest_WhenReportDtoIsNull()
    {
        // Arrange
        _mockReportService.Setup(service => service.Add(null)).ThrowsAsync(new ArgumentNullException());

        // Act
        var result = await _reportController.Add(null);

        // ClassicAssert
        ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        ClassicAssert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        ClassicAssert.IsInstanceOf<ErrorModel>(badRequestResult.Value);
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