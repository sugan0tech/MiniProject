using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.MatchRequest;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
[Authorize]
public class MatchRequestController(IMatchRequestService matchRequestService, ILogger<MatchRequestController> logger)
    : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var match = await matchRequestService.GetById(id);
            return Ok(match);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("accepted/{profileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAcceptedMfalseatches(int profileId)
    {
        var matches = await matchRequestService.GetAcceptedMatchRequests(profileId);
        return Ok(matches);
    }

    [HttpGet("/profile/{profileId}")]
    [ProducesResponseType(typeof(List<MatchRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMatchRequests(int profileId)
    {
        var matches = await matchRequestService.GetMatchRequests(profileId);
        return Ok(matches);
    }

    [HttpGet("sent/{profileId}")]
    [ProducesResponseType(typeof(List<MatchRequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSentMatchRequests(int profileId)
    {
        try
        {
            var matches = await matchRequestService.GetSentMatchRequests(profileId);
            return Ok(matches);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            return NotFound(new ErrorModel(404, e.Message));
        }
    }

    [HttpPost("reject/{matchId}/{profileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject(int matchId, int profileId)
    {
        try
        {
            await matchRequestService.Reject(matchId, profileId);
            return Ok();
        }
        catch (InvalidMatchForProfile ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPost("approve/{matchId}/{profileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(int matchId, int profileId)
    {
        try
        {
            await matchRequestService.Approve(matchId, profileId);
            return Ok();
        }
        catch (InvalidMatchForProfile ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPost("{senderId}/{targetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MatchRequestToProfile(int senderId, int targetId)
    {
        try
        {
            var match = await matchRequestService.MatchRequestToProfile(senderId, targetId);
            return Ok(match);
        }
        catch (DbUpdateException e)
        {
            var message = e.Message;
            if (e.InnerException != null)
                message = e.InnerException.Message;

            return BadRequest(new ErrorModel(400, message));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ErrorModel(404, e.Message));
        }
        catch (DuplicateRequestException e)
        {
            return BadRequest(new ErrorModel(400, e.Message));
        }
        catch (MatchRequestToSelfException e)
        {
            return BadRequest(new ErrorModel(400, e.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int id)
    {
        try
        {
            var match = await matchRequestService.DeleteById(id);
            return Ok(match);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            return NotFound(new ErrorModel(404, e.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAll()
    {
        var matches = await matchRequestService.GetAll();
        return Ok(matches);
    }
}