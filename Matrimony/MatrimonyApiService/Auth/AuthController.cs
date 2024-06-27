using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.User;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace MatrimonyApiService.Auth;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var loginReturnDto = await authService.Login(loginDto);
            logger.LogInformation(loginDto.Email);
            logger.LogCritical(loginDto.Email);
            return Ok(loginReturnDto);
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

    [HttpPost("logout")]
    [ProducesResponseType(typeof(Ok), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Logout()
    {
        try
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                var token = authHeader.ToString().Split(' ').Last();
                await authService.Logout(token);
                return Ok();
            }

            return BadRequest(new ErrorModel(400, "Authorization header not found."));
        }
        catch (AuthenticationException e)
        {
            return BadRequest(new ErrorModel(400, e.Message));
        }
    }

    [HttpGet("access-token")]
    [ProducesResponseType(typeof(AuthReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAccessToken()
    {
        try
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                var token = authHeader.ToString().Split(' ').Last();
                var dto = await authService.GetAccessToken(token);
                return Ok(dto);
            }

            return BadRequest(new ErrorModel(400, "Authorization header not found."));
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
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            var user = await authService.Register(registerDto);
            return Ok(user);
        }
        catch (DbUpdateException e)
        {
            var message = e.Message;
            if (e.InnerException != null)
                message = e.InnerException.Message;

            return BadRequest(new ErrorModel(400, message));
        }
        catch (UserNotVerifiedException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
        catch (AuthenticationException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
        }
    }
    
    [HttpPost("verify-otp/{userId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyOtp([FromBody] string otp, int userId)
    {
        try
        {
            var success = await authService.VerifyUserByOtp(userId, otp);
            if (success)
                return Ok(new { Success = success });
            return BadRequest(new ErrorModel(400, "Invalid Wrong OTP"));
        }
        catch (UserNotVerifiedException e)
        {
            return Unauthorized(new ErrorModel(401, e.Message));
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
            var dto = await authService.ResetPassword(resetPasswordDto);
            return Ok(dto);
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