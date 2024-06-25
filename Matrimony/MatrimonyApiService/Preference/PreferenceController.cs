using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Preference;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowAll")]
[Authorize]
public class PreferenceController(IPreferenceService preferenceService, ILogger<PreferenceController> logger)
    : ControllerBase
{
    /// <summary>
    /// Adds a new preference.
    /// </summary>
    /// <param name="preferenceDto">Preference data transfer object</param>
    /// <returns>Added preference</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PreferenceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(PreferenceDto preferenceDto)
    {
        try
        {
            var addedPreference = await preferenceService.Add(preferenceDto);
            return Ok(addedPreference);
        }
        catch (ValidationException e)
        {
            logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Gets a preference by its unique identifier.
    /// </summary>
    /// <param name="id">Preference ID</param>
    /// <returns>Preference data transfer object</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PreferenceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var preference = await preferenceService.GetById(id);
            return Ok(preference);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    /// <summary>
    /// Updates an existing preference.
    /// </summary>
    /// <param name="preferenceDto">Preference data transfer object</param>
    /// <returns>Updated preference</returns>
    [HttpPut]
    [ProducesResponseType(typeof(PreferenceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(PreferenceDto preferenceDto)
    {
        try
        {
            var updatedPreference = await preferenceService.Update(preferenceDto);
            return Ok(updatedPreference);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (ValidationException e)
        {
            logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}