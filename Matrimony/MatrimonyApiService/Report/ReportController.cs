using MatrimonyApiService.Commons;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Report;

[ApiController]
[Route("api/[controller]")]
public class ReportController(IBaseService<Report, ReportDto> reportService, ILogger<ReportController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var reports = await reportService.GetAll();
        return Ok(reports);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(ReportDto report)
    {
        try
        {
            var addedReport = await reportService.Add(report);
            return Ok(addedReport);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(ReportDto report)
    {
        try
        {
            var updatedReport = await reportService.Update(report);
            return Ok(updatedReport);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
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