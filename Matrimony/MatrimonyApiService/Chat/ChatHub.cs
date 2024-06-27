using Microsoft.AspNetCore.SignalR;

namespace MatrimonyApiService.Chat
{
    public class ChatHub(ChatService chatService, NewMessageService messageService) : Hub
    {

        public async Task SendMessage(string chatId, string senderId, string messageContent)
        {
            int chatIdInt = int.Parse(chatId);

            // Create a new message
            var message = new Message.Message
            {
                ChatId = chatIdInt,
                SenderId = int.Parse(senderId),
                SentAt = DateTime.UtcNow,
                Seen = false,
                // Assuming that the ReceiverId is determined by your application logic
            };

            // Save the message to the database
            await messageService.CreateMessageAsync(message);

            // Update the last message timestamp for the chat
            var chat = await chatService.GetChatByIdAsync(chatIdInt);
            if (chat != null)
            {
                chat.LastMessageAt = message.SentAt;
                await chatService.UpdateChatAsync(chat);
            }

            // Broadcast the message to all clients in the specified chat
            await Clients.Group(chatId).SendAsync("ReceiveMessage", senderId, messageContent);
        }

        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
    }
}