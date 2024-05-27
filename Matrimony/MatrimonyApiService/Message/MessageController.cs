using System.ComponentModel.DataAnnotations;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Message;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService, ILogger<MessageController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddMessage(MessageDto messageDto)
    {
        var addedMessage = await messageService.AddMessage(messageDto);
        return Ok(addedMessage);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessageById(int id)
    {
        try
        {
            var message = await messageService.GetMessageById(id);
            return Ok(message);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            return NotFound(new ErrorModel(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMessage(MessageDto messageDto)
    {
        try
        {
            var updatedMessage = await messageService.UpdateMessage(messageDto);
            return Ok(updatedMessage);
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await messageService.GetAllMessages();
        return Ok(messages);
    }

    [HttpGet("sent/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetSentMessages(int userId)
    {
        try
        {
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
    }

    [HttpGet("received/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReceivedMessages(int userId)
    {
        try
        {
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
    }
}