using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using MatrimonyApiService.MatchRequest;
using MatrimonyApiService.Profile.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Profile;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
[Authorize]
public class ProfileController(
    IProfileService profileService,
    CustomControllerValidator validator,
    IMediator mediator,
    ILogger<ProfileController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileById(int id)
    {
        try
        {
            var profile = await profileService.GetProfileById(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("user")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetProfileByUserId()
    {
        try
        {
            var userId = validator.ValidateAndGetUserId(User.Claims);
            var profile = await profileService.GetProfileByUserId(userId);
            if (profile == null)
            {
                throw new KeyNotFoundException("No profile found for this user");
            }
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
    }

    [HttpGet("manager")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetProfilesForManager()
    {
        try
        {
            var managerId = validator.ValidateAndGetUserId(User.Claims);
            var profiles = await profileService.GetProfilesByManager(managerId);
            return Ok(profiles);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
    }

    [HttpGet("preview/{id}")]
    [ProducesResponseType(typeof(ProfilePreviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfilePreviewById(int id)
    {
        try
        {
            var profilePreview = await profileService.GetProfilePreviewById(id);
            return Ok(profilePreview);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddProfile(ProfileDto profileDto)
    {
        try
        {
            validator.ValidateUserPrivilege(User.Claims, profileDto.ManagedById);
            var command = new CreateProfileCommand(profileDto);
            var value = await mediator.Send(command);
            return StatusCode(201, value);
        }
        catch (DuplicateRequestException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
        catch (DbUpdateException ex)
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


    [HttpPut]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
    {
        try
        {
            validator.ValidateUserPrivilege(User.Claims, (profileDto.ManagedById, profileDto.UserId));
            var profile = await profileService.UpdateProfile(profileDto);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
    }

    [HttpDelete("{profileId}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProfileById(int profileId)
    {
        try
        {
            await validator.ValidateUserPrivilegeForProfile(User.Claims, profileId);
            var profile = await mediator.Send(new DeleteProfileCommand(profileId));
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, new ErrorModel(StatusCodes.Status500InternalServerError, ex.Message));
        }
    }

    [HttpGet("{profileId}/matches")]
    [ProducesResponseType(typeof(List<MatchRequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMatches(int profileId)
    {
        try
        {
            var matches = await profileService.GetProfileMatches(profileId);
            return Ok(matches);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (EntityNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProfileDto>), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await profileService.GetAll();
        return Ok(profiles);
    }
}