using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Report;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController(
    IBaseService<Report, ReportDto> reportService,
    CustomControllerValidator validator,
    ILogger<ReportController> logger)
    : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var report = await reportService.GetById(id);
            return Ok(report);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReportDto>), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAll()
    {
        var reports = await reportService.GetAll();
        return Ok(reports);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add(ReportDto report)
    {
        try
        {
            validator.ValidateUserPrivilege(User.Claims, report.ReportedById);
            var addedReport = await reportService.Add(report);
            return Ok(addedReport);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteById(int id)
    {
        try
        {
            var deletedReport = await reportService.DeleteById(id);
            return Ok(deletedReport);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }
}