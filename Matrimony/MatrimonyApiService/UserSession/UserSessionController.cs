using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace MatrimonyApiService.UserSession;

[Route("api/[controller]")]
[ApiController]
[EnableCors("AllowAll")]
[Authorize]
public class UserSessionController(
    IUserSessionService userSessionService,
    CustomControllerValidator validator,
    ILogger<UserSessionController> logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<UserSessionDto>), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAll()
    {
        var sessions = await userSessionService.GetAll();
        return Ok(sessions);
    }

    [HttpGet("{sessionId}")]
    [ProducesResponseType(typeof(UserSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int sessionId)
    {
        try
        {
            var session = await userSessionService.GetById(sessionId);
            return Ok(session);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("user")]
    [ProducesResponseType(typeof(List<UserSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    public IActionResult GetByUserId()
    {
        try
        {
            var sessions = userSessionService.GetByUserId(validator.ValidateAndGetUserId(User.Claims));
            return Ok(sessions);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status401Unauthorized, ex.Message));
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserSessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] UserSessionDto userSessionDto)
    {
        try
        {
            var createdSession = await userSessionService.Add(userSessionDto);
            return StatusCode(StatusCodes.Status201Created, createdSession);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ValidationResult(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UserSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int id)
    {
        try
        {
            var deletedSession = await userSessionService.DeleteById(id);
            return Ok(deletedSession);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("invalidate/{token}")]
    [ProducesResponseType(typeof(UserSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Invalidate(string token)
    {
        try
        {
            var invalidatedSession = await userSessionService.Invalidate(token);
            return Ok(invalidatedSession);
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("invalidateAll/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InvalidateAllPerUser(int userId)
    {
        try
        {
            await userSessionService.InvalidateAllPerUser(userId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("validateToken/{token}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IsValid(string token)
    {
        try
        {
            var isValid = await userSessionService.IsValid(token);
            return Ok(new { IsValid = isValid });
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("flush")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Flush()
    {
        await userSessionService.Flush();
        return Ok();
    }
}