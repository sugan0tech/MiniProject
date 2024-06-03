using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Membership;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class MembershipController(IMembershipService membershipService, ILogger<MembershipController> logger)
    : ControllerBase
{
    [HttpGet("profile/{profileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProfileId(int profileId)
    {
        try
        {
            var membership = await membershipService.GetByProfileId(profileId);
            return Ok(membership);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(404, ex.Message));
        }
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        try
        {
            var membership = await membershipService.GetByUserId(userId);
            return Ok(membership);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(404, ex.Message));
        }
    }

    [HttpDelete("{membershipId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(int membershipId)
    {
        try
        {
            var deletedMembership = await membershipService.DeleteById(membershipId);
            return Ok(deletedMembership);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(MembershipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(MembershipDto dto)
    {
        try
        {
            var addedMembership = await membershipService.Add(dto);
            return Ok(addedMembership);
        }
        catch (AlreadyExistingEntityException ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(new ErrorModel(400, ex.Message));
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(MembershipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(MembershipDto dto)
    {
        try
        {
            var updatedMembership = await membershipService.Update(dto);
            return Ok(updatedMembership);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("validate/{membershipId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Validate(int membershipId)
    {
        try
        {
            await membershipService.Validate(membershipId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Validate(MembershipDto dto)
    {
        try
        {
            await membershipService.Validate(dto);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPost("validate/all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateAll()
    {
        await membershipService.ValidateAll();
        return Ok();
    }
}