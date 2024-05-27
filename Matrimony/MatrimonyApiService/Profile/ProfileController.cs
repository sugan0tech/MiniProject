using MatrimonyApiService.Commons;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Profile;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(IProfileService profileService, ILogger<ProfileController> logger ): ControllerBase
{

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProfile(ProfileDto profileDto)
    {
        try
        {
            var profile = await profileService.AddProfile(profileDto);
            return CreatedAtAction(nameof(GetProfileById), new { id = profile.ProfileId }, profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await profileService.GetAll();
        return Ok(profiles);
    }
}