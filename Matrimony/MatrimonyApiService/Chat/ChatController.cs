using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Validations;
using MatrimonyApiService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Chat;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableCors("AllowAll")]
public class ChatController(ChatService chatService, CustomControllerValidator validator) : ControllerBase
{
    [HttpPost("{senderId}/{receiverId}")]
    public async Task<IActionResult> NewChat(int senderId, int receiverId)
    {
        try
        {
            await validator.ValidateUserPrivilegeForProfile(User.Claims, senderId);
            var chats = await chatService.CreateChatAsync(senderId, receiverId);

            return Ok(chats);
        }
        catch (AuthenticationException)
        {
            try
            {
                await validator.ValidateUserPrivilegeForProfile(User.Claims, receiverId);
                var chats = await chatService.CreateChatAsync(senderId, receiverId);

                return Ok(chats);
            }
            catch (AuthenticationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorModel(403, e.Message));
            }
            catch (DuplicateRequestException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, e.Message));
            }
        }
        catch (DuplicateRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorModel(400, e.Message));
        }
    }

    [HttpGet("findChat/{senderId}/{receiverId}")]
    public async Task<IActionResult> FindChat(int senderId, int receiverId)
    {
        try
        {
            await validator.ValidateUserPrivilegeForProfile(User.Claims, senderId);
            var chat = await chatService.FindChatByParticipantsAsync(senderId, receiverId);

            return Ok(chat);
        }
        catch (AuthenticationException)
        {
            try
            {
                await validator.ValidateUserPrivilegeForProfile(User.Claims, receiverId);
                var chat = await chatService.FindChatByParticipantsAsync(senderId, receiverId);

                return Ok(chat);
            }
            catch (EntityNotFoundException e)
            {
                Console.WriteLine(e);
                return NotFound(new ErrorModel(404, "Chat not found exception"));
            }
            catch (AuthenticationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }
    }

    [HttpGet("chats/{profileId}")]
    public async Task<IActionResult> GetChats(int profileId)
    {
        await validator.ValidateUserPrivilegeForProfile(User.Claims, profileId);
        var chats = await chatService.GetChatsByUserIdAsync(profileId);
        if (chats.Count == 0)
        {
            return StatusCode(StatusCodes.Status404NotFound, new ErrorModel(404, "No Chats are there"));
        }

        return Ok(chats);
    }

    [HttpGet("messages/{chatId}")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        var messages = await chatService.GetMessagesByChatIdAsync(chatId);
        return Ok(messages);
    }
}