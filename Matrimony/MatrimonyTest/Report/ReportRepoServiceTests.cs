using AutoMapper;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Report;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Report;

public class ReportServiceTests
{
    private Mock<IBaseRepo<MatrimonyApiService.Report.Report>> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<BaseService<MatrimonyApiService.Report.Report, ReportDto>>> _mockLogger;
    private ReportService _reportService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IBaseRepo<MatrimonyApiService.Report.Report>>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<BaseService<MatrimonyApiService.Report.Report, ReportDto>>>();
        _reportService = new ReportService(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetById_ShouldReturnReportDto_WhenReportExists()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report { Id = 1, ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = report.ReportedAt };

        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(report);
        _mockMapper.Setup(mapper => mapper.Map<ReportDto>(It.IsAny<MatrimonyApiService.Report.Report>())).Returns(reportDto);

        // Act
        var result = await _reportService.GetById(1);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(reportDto.ProfileId, result.ProfileId);
        _mockRepo.Verify(repo => repo.GetById(1), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto>(report), Times.Once);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenReportDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        ClassicAssert.ThrowsAsync<KeyNotFoundException>(async () => await _reportService.GetById(99));
        _mockRepo.Verify(repo => repo.GetById(99), Times.Once);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllReports()
    {
        // Arrange
        var reports = new List<MatrimonyApiService.Report.Report>
        {
            new MatrimonyApiService.Report.Report { Id = 1, ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now },
            new MatrimonyApiService.Report.Report { Id = 2, ProfileId = 2, ReportedById = 2, ReportedAt = DateTime.Now }
        };
        var reportDtos = new List<ReportDto>
        {
            new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now },
            new ReportDto { ProfileId = 2, ReportedById = 2, ReportedAt = DateTime.Now }
        };

        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(reports);
        _mockMapper.Setup(mapper => mapper.Map<ReportDto>(It.IsAny<MatrimonyApiService.Report.Report>()))
            .Returns((MatrimonyApiService.Report.Report src) => new ReportDto { ProfileId = src.ProfileId, ReportedById = src.ReportedById, ReportedAt = src.ReportedAt });

        // Act
        var result = await _reportService.GetAll();

        // ClassicAssert
        ClassicAssert.AreEqual(2, result.Count);
        _mockRepo.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Test]
    public async Task Add_ShouldAddReport()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report { Id = 1, ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = report.ReportedAt };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Report.Report>(It.IsAny<ReportDto>())).Returns(report);
        _mockRepo.Setup(repo => repo.Add(It.IsAny<MatrimonyApiService.Report.Report>())).ReturnsAsync(report);
        _mockMapper.Setup(mapper => mapper.Map<ReportDto>(It.IsAny<MatrimonyApiService.Report.Report>())).Returns(reportDto);

        // Act
        var result = await _reportService.Add(reportDto);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(reportDto.ProfileId, result.ProfileId);
        _mockRepo.Verify(repo => repo.Add(report), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto>(report), Times.Once);
    }

    [Test]
    public async Task Update_ShouldUpdateReport()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report { Id = 1, ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = report.ReportedAt };

        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Report.Report>(It.IsAny<ReportDto>())).Returns(report);
        _mockRepo.Setup(repo => repo.Update(It.IsAny<MatrimonyApiService.Report.Report>())).ReturnsAsync(report);
        _mockMapper.Setup(mapper => mapper.Map<ReportDto>(It.IsAny<MatrimonyApiService.Report.Report>())).Returns(reportDto);

        // Act
        var result = await _reportService.Update(reportDto);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(reportDto.ProfileId, result.ProfileId);
        _mockRepo.Verify(repo => repo.Update(report), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto>(report), Times.Once);
    }

    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenReportDoesNotExist()
    {
        // Arrange
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        _mockMapper.Setup(mapper => mapper.Map<MatrimonyApiService.Report.Report>(It.IsAny<ReportDto>())).Returns(new MatrimonyApiService.Report.Report
        {
            ProfileId = 1,
            ReportedById = 2
        });
        _mockRepo.Setup(repo => repo.Update(It.IsAny<MatrimonyApiService.Report.Report>())).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reportService.Update(reportDto));
        _mockRepo.Verify(repo => repo.Update(It.IsAny<MatrimonyApiService.Report.Report>()), Times.Once);
    }

    [Test]
    public async Task DeleteById_ShouldRemoveReport()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report { Id = 1, ProfileId = 1, ReportedById = 1, ReportedAt = DateTime.Now };
        var reportDto = new ReportDto { ProfileId = 1, ReportedById = 1, ReportedAt = report.ReportedAt };

        _mockRepo.Setup(repo => repo.DeleteById(It.IsAny<int>())).ReturnsAsync(report);
        _mockMapper.Setup(mapper => mapper.Map<ReportDto>(It.IsAny<MatrimonyApiService.Report.Report>())).Returns(reportDto);

        // Act
        var result = await _reportService.DeleteById(1);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(reportDto.ProfileId, result.ProfileId);
        _mockRepo.Verify(repo => repo.DeleteById(1), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto>(report), Times.Once);
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenReportDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.DeleteById(It.IsAny<int>())).ThrowsAsync(new KeyNotFoundException());

        // Act & ClassicAssert
        ClassicAssert.ThrowsAsync<KeyNotFoundException>(async () => await _reportService.DeleteById(99));
        _mockRepo.Verify(repo => repo.DeleteById(99), Times.Once);
    }
}