using Microsoft.AspNetCore.Mvc;

namespace MatrimonyApiService.Chat;

[ApiController]
[Route("api/[controller]")]
public class ChatController(ChatService chatService): ControllerBase
{

    [HttpGet("chats/{profileId}")]
    public async Task<IActionResult> GetChats(int profileId)
    {
        var chats = await chatService.GetChatsByUserIdAsync(profileId);
        if (chats == null)
        {
            return NotFound();
        }

        return Ok(chats);
    }

    [HttpGet("messages/{chatId}")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        var messages = await chatService.GetMessagesByChatIdAsync(chatId);
        if (messages == null)
        {
            return NotFound();
        }

        return Ok(messages);
    }
}