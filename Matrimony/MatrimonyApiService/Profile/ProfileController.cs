using MatrimonyApiService.Commons;
using MatrimonyApiService.Match;
using MatrimonyApiService.ProfileView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Profile;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(IProfileService profileService, ILogger<ProfileController> logger) : ControllerBase
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

    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        try
        {
            var profile = await profileService.GetProfileByUserId(userId);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("manager/{managerId}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfilePreviewForManager(int managerId)
    {
        try
        {
            var profiles = await profileService.GetProfilePreviewForManager(managerId);
            return Ok(profiles);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
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
    public async Task<IActionResult> AddProfile(ProfileDto profileDto)
    {
        try
        {
            var profile = await profileService.AddProfile(profileDto);
            return StatusCode(201, profile);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(ProfileDto profileDto)
    {
        try
        {
            var profile = await profileService.UpdateProfile(profileDto);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfileById(int id)
    {
        try
        {
            var profile = await profileService.DeleteProfileById(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("{profileId}/matches")]
    [ProducesResponseType(typeof(List<MatchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMatches(int profileId)
    {
        try
        {
            var matches = await profileService.GetMatches(profileId);
            return Ok(matches);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet("{profileId}/views")]
    [ProducesResponseType(typeof(List<ProfileViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetViews(int profileId)
    {
        try
        {
            var views = await profileService.GetViews(profileId);
            return Ok(views);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await profileService.GetAll();
        return Ok(profiles);
    }
}