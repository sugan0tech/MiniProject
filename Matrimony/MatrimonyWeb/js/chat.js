// Dummy data, soon will be integrated
const chats = [
    {
        chatId: '1',
        participants: ['1', '2'],
        lastMessage: {
            sender: '1',
            content: 'How are you?',
            timestamp: '2024-06-26T12:00:00Z'
        },
        unreadCount: 2
    },
    {
        chatId: '2',
        participants: ['1', '3'],
        lastMessage: {
            sender: '3',
            content: 'Good morning!',
            timestamp: '2024-06-26T08:00:00Z'
        },
        unreadCount: 0
    }
];

const messages = {
    '1': [
        {
            messageId: '1',
            chatId: '1',
            sender: '1',
            receiver: '2',
            content: 'Hello!',
            timestamp: '2024-06-26T10:00:00Z',
            isRead: true
        },
        {
            messageId: '2',
            chatId: '1',
            sender: '2',
            receiver: '1',
            content: 'Hi! How are you?',
            timestamp: '2024-06-26T11:00:00Z',
            isRead: true
        },
        {
            messageId: '3',
            chatId: '1',
            sender: '1',
            receiver: '2',
            content: 'I am fine, thank you. And you?',
            timestamp: '2024-06-26T12:00:00Z',
            isRead: false
        }
    ],
    '2': [
        {
            messageId: '4',
            chatId: '2',
            sender: '3',
            receiver: '1',
            content: 'Good morning!',
            timestamp: '2024-06-26T08:00:00Z',
            isRead: true
        },
        {
            messageId: '5',
            chatId: '2',
            sender: '1',
            receiver: '3',
            content: 'Morning! How was your night?',
            timestamp: '2024-06-26T08:30:00Z',
            isRead: true
        }
    ]
};

// Function to load chats
function loadChats() {
    const chatListElement = document.getElementById('chatList');
    chatListElement.innerHTML = '';
    chats.forEach(chat => {
        const chatItem = document.createElement('div');
        chatItem.className = 'chat-item';
        chatItem.dataset.chatId = chat.chatId;
        chatItem.innerHTML = `
                <h6>Chat with ${chat.participants.find(p => p !== '1')}</h6>
                <p>${chat.lastMessage.content}</p>
                <span class="badge bg-primary">${chat.unreadCount}</span>
            `;
        chatItem.addEventListener('click', () => selectChat(chatItem, chat.chatId));
        chatListElement.appendChild(chatItem);
    });
}

// Function to select a chat
function selectChat(chatItem, chatId) {
    // Remove 'selected' class from all chat items
    document.querySelectorAll('.chat-item').forEach(item => item.classList.remove('selected'));
    // Add 'selected' class to the clicked chat item
    chatItem.classList.add('selected');
    // Load messages for the selected chat
    loadMessages(chatId);
}

// Function to load messages for a chat
function loadMessages(chatId) {
    const messageListElement = document.getElementById('messageList');
    messageListElement.innerHTML = '';
    if (messages[chatId]) {
        messages[chatId].forEach(message => {
            const messageElement = document.createElement('div');
            messageElement.className = `message ${message.sender === '1' ? 'sender' : 'receiver'}`;
            messageElement.innerHTML = `
                    <p>${message.content}</p>
                    <small>${new Date(message.timestamp).toLocaleTimeString()}</small>
                `;
            messageListElement.appendChild(messageElement);
        });
    }
}