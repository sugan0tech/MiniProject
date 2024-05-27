using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.Profile;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.User;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
{

    [HttpGet("{userId}/view-profile/{profileId}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ViewProfile(int userId, int profileId)
    {
        try
        {
            var profile = await userService.ViewProfile(userId, profileId);
            return Ok(profile);
        }
        catch (NonPremiumUserException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
        catch (ExhaustedMaximumProfileViewsException ex)
        {
            logger.LogWarning(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAll();
        return Ok(users);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] User user)
    {
        var createdUser = await userService.Add(user);
        return StatusCode(StatusCodes.Status201Created, createdUser);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        try
        {
            var updatedUser = await userService.Update(user);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            var user = await userService.GetByEmail(email);
            return Ok(user);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int id)
    {
        try
        {
            var deletedUser = await userService.DeleteById(id);
            return Ok(deletedUser);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }
}