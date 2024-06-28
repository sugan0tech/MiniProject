using Microsoft.AspNetCore.SignalR;

namespace MatrimonyApiService.Chat
{
    public class ChatHub(ChatService chatService, NewMessageService messageService) : Hub
    {

        public async Task SendMessage(string chatId, string senderId, string receiverId, string messageContent)
        {
            int chatIdInt = int.Parse(chatId);
            // Create a new message
            var message = new Message.Message
            {
                ChatId = chatIdInt,
                SenderId = int.Parse(senderId),
                ReceiverId = int.Parse(receiverId),
                SentAt = DateTime.UtcNow,
                Seen = false,
                Content = messageContent
            };

            // Save the message to the database
            await messageService.CreateMessageAsync(message);

            var chat = await chatService.GetChatByIdAsync(chatIdInt);
            
            if (chat != null)
            {
                chat.LastMessageAt = message.SentAt;
                await chatService.UpdateChatAsync(chat);
            }

            await Clients.Group(chatId).SendAsync("ReceiveMessage", senderId, messageContent);
        }

        public async Task SeenMessage(string chatId, string messageId)
        {
            var msgIdInt = int.Parse(messageId);
            var message = await messageService.GetById(msgIdInt);
            message.Seen = true;
            await messageService.UpdateMessageAsync(message);
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