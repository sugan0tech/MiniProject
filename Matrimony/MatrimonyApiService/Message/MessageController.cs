using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.Message;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController(IMessageService messageService, ILogger<MessageController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddMessage(MessageDto messageDto)
    {
        try
        {
            ControllerValidator.ValidateUserPrivilege(User.Claims, messageDto.SenderId);
            var addedMessage = await messageService.AddMessage(messageDto);
            return Ok(addedMessage);
        }
        catch (DbUpdateException e)
        {
            var message = e.Message;
            if (e.InnerException != null)
                message = e.InnerException.Message;

            return BadRequest(new ErrorModel(400, message));
        }
        catch (AuthenticationException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(403, new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMessageById(int id)
    {
        try
        {
            var message = await messageService.GetMessageById(id);
            ControllerValidator.ValidateUserPrivilege(User.Claims, (message.SenderId, message.ReceiverId));
            return Ok(message);
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

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> DeleteMessageById(int id)
    {
        try
        {
            var deletedMessage = await messageService.DeleteMessageById(id);
            return Ok(deletedMessage);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await messageService.GetAllMessages();
        return Ok(messages);
    }

    [HttpGet("sent/{userId}")]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetSentMessages(int userId)
    {
        try
        {
            ControllerValidator.ValidateUserPrivilege(User.Claims, userId);
            var messages = await messageService.GetSentMessages(userId);
            return Ok(messages);
        }
        catch (NonPremiumUserException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden,
                new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
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

    [HttpGet("received/{userId}")]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReceivedMessages(int userId)
    {
        try
        {
            ControllerValidator.ValidateUserPrivilege(User.Claims, userId);
            var messages = await messageService.GetReceivedMessages(userId);
            return Ok(messages);
        }
        catch (NonPremiumUserException ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status403Forbidden,
                new ErrorModel(StatusCodes.Status403Forbidden, ex.Message));
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
}