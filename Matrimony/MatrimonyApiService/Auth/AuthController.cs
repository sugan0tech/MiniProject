using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthReturnDto>> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var loginReturnDto = await authService.Login(loginDto);
            logger.LogInformation(loginDto.Email);
            logger.LogCritical(loginDto.Email);
            return Ok(loginReturnDto);
        }
        catch (ValidationException e)
        {
            return BadRequest(new ErrorModel(400, e.Message));
        }
        catch (UserNotVerifiedException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
        catch (UserNotFoundException e)
        {
            return NotFound(new ErrorModel(404, e.Message));
        }
        catch (AuthenticationException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            var success = await authService.Register(registerDto);
            return Ok(new { Success = success });
        }
        catch (UserNotVerifiedException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
        catch (UserNotFoundException e)
        {
            return NotFound(new ErrorModel(404, e.Message));
        }
        catch (AuthenticationException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
    }

    [HttpPost("ResetPassword")]
    [ProducesResponseType(typeof(AuthReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        try
        {
            await authService.ResetPassword(resetPasswordDto);
            return Ok("Password reset successful");
        }
        catch (UserNotVerifiedException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
        catch (UserNotFoundException e)
        {
            return NotFound(new ErrorModel(404, e.Message));
        }
        catch (AuthenticationException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
    }
}