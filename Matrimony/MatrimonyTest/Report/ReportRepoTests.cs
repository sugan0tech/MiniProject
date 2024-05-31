using Microsoft.EntityFrameworkCore;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Report;
using NUnit.Framework.Legacy;

namespace MatrimonyTest.Report;

[TestFixture]
public class ReportRepoTests
{
    private DbContextOptions<MatrimonyContext> _dbContextOptions;
    private MatrimonyContext _context;
    private ReportRepo _reportRepo;

    [SetUp]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<MatrimonyContext>()
            .UseInMemoryDatabase("MatrimonyTestDb")
            .Options;

        _context = new MatrimonyContext(_dbContextOptions);
        _reportRepo = new ReportRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetById_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report
        {
            ProfileId = 1,
            ReportedById = 1,
            ReportedAt = DateTime.Now
        };
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reportRepo.GetById(report.Id);

        // ClassicAssert
        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(report.ProfileId, result.ProfileId);
        ClassicAssert.AreEqual(report.ReportedById, result.ReportedById);
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reportRepo.GetById(99));
        ClassicAssert.AreEqual("Report with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        await _context.Reports.AddRangeAsync(
            new MatrimonyApiService.Report.Report
            {
                ProfileId = 1,
                ReportedById = 1,
                ReportedAt = DateTime.Now
            },
            new MatrimonyApiService.Report.Report
            {
                ProfileId = 2,
                ReportedById = 2,
                ReportedAt = DateTime.Now
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _reportRepo.GetAll();

        // ClassicAssert
        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task Add_ShouldAddEntity()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report
        {
            ProfileId = 1,
            ReportedById = 1,
            ReportedAt = DateTime.Now
        };

        // Act
        var result = await _reportRepo.Add(report);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(report.ProfileId, result.ProfileId);
        ClassicAssert.AreEqual(report.ReportedById, result.ReportedById);
        ClassicAssert.AreEqual(1, await _context.Reports.CountAsync());
    }

    [Test]
    public void Add_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _reportRepo.Add(null));
        ClassicAssert.AreEqual("Report cannot be null. (Parameter 'entity')", ex.Message);
    }

    [Test]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report
        {
            ProfileId = 1,
            ReportedById = 1,
            ReportedAt = DateTime.Now
        };
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        // Act
        report.ReportedById = 2;
        var result = await _reportRepo.Update(report);

        // ClassicAssert
        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(2, result.ReportedById);
    }

    [Test]
    public void Update_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report
        {
            Id = 99,
            ProfileId = 1,
            ReportedById = 1,
            ReportedAt = DateTime.Now
        };

        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reportRepo.Update(report));
        ClassicAssert.AreEqual("Report with key 99 not found!!!", ex.Message);
    }

    [Test]
    public async Task DeleteById_ShouldRemoveEntity()
    {
        // Arrange
        var report = new MatrimonyApiService.Report.Report
        {
            ProfileId = 1,
            ReportedById = 1,
            ReportedAt = DateTime.Now
        };
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();

        // Act
        await _reportRepo.DeleteById(report.Id);

        // ClassicAssert
        var result = await _reportRepo.GetAll();
        ClassicAssert.IsEmpty(result);
    }

    [Test]
    public void DeleteById_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Act & ClassicAssert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reportRepo.DeleteById(99));
        ClassicAssert.AreEqual("Report with key 99 not found!!!", ex.Message);
    }
}