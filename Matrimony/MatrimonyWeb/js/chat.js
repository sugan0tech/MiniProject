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
async function loadChats() {
    const chatListElement = document.getElementById('chatList');
    chatListElement.innerHTML = ''; // Clear existing chat list

    try {
        // Fetch chat data from API
        const chats = await makeAuthRequest('/api/chat/chats', 'GET');

        if (!chats) {
            showAlert('Failed to load chats', 'danger');
            return;
        }

        // Iterate over each chat and create chat elements
        chats.forEach(chat => {
            const chatItem = document.createElement('div');
            chatItem.className = 'chat-item';
            chatItem.dataset.chatId = chat.chatId;
            chatItem.innerHTML = `
                <h6>Chat with ${chat.participants.find(p => p !== '1')}</h6>
                <p>${chat.lastMessage ? chat.lastMessage.content : 'No messages yet'}</p>
                <span class="badge bg-primary">${chat.unreadCount}</span>
            `;
            chatItem.addEventListener('click', () => selectChat(chatItem, chat.chatId));
            chatListElement.appendChild(chatItem);
        });
    } catch (error) {
        console.error('Error loading chats:', error);
        showAlert('Failed to load chats', 'danger');
    }
}

function selectChat(chatItem, chatId) {
    // Remove 'selected' class from all chat items
    document.querySelectorAll('.chat-item').forEach(item => item.classList.remove('selected'));
    // Add 'selected' class to the clicked chat item
    chatItem.classList.add('selected');
    // Load messages for the selected chat
    loadMessages(chatId);
}

// Function to load messages for a chat
async function loadMessages(chatId) {
    const messageListElement = document.getElementById('messageList');
    messageListElement.innerHTML = ''; // Clear existing messages

    try {
        // Fetch messages for the selected chat from API
        const messages = await makeAuthRequest(`/api/chat/messages/${chatId}`, 'GET');

        if (!messages) {
            showAlert('Failed to load messages', 'danger');
            return;
        }

        // Iterate over each message and create message elements
        messages.forEach(message => {
            const messageElement = document.createElement('div');
            messageElement.className = `message ${message.senderId === '1' ? 'sender' : 'receiver'}`;
            messageElement.innerHTML = `
                <p>${message.content}</p>
                <small>${new Date(message.sentAt).toLocaleTimeString()}</small>
            `;
            messageListElement.appendChild(messageElement);
        });
    } catch (error) {
        console.error('Error loading messages:', error);
        showAlert('Failed to load messages', 'danger');
    }
}




// Call loadChats when the page loads
document.addEventListener('DOMContentLoaded', loadChats);

// Create a connection to the hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Start the connection
async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.error(err.toString());
        setTimeout(start, 5000);
    }
}

// Join a chat room
function joinChat(chatId) {
    connection.invoke("JoinChat", chatId).catch(err => console.error(err.toString()));
}

// Leave a chat room
function leaveChat(chatId) {
    connection.invoke("LeaveChat", chatId).catch(err => console.error(err.toString()));
}

// Send a message
document.getElementById("sendButton").addEventListener("click", function () {
    const message = document.getElementById("messageInput").value;
    const chatId = "chat123"; // Replace with the actual chat ID
    const senderId = "user1"; // Replace with the actual sender ID
    connection.invoke("SendMessage", chatId, senderId, message).catch(err => console.error(err.toString()));
    document.getElementById("messageInput").value = '';
});

// Receive messages
connection.on("ReceiveMessage", function (senderId, message) {
    const msg = document.createElement("div");
    msg.textContent = `${senderId}: ${message}`;
    document.getElementById("chatContainer").appendChild(msg);
});

// Start the connection
start();
