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

    const currentChat = JSON.parse(localStorage.getItem("currentChat"));

    try {
        // Fetch chat data from API
        let chats = await makeAuthRequest('chat/chats/' + localStorage.getItem("currentProfile"), 'GET');


        if (!chats) {
            showAlert('NO chats found', 'warning');
            return;
        }

        // Iterate over each chat and create chat elements
        chats.forEach(chat => {
            const chatItem = document.createElement('div');
            chatItem.className = 'chat-item';
            chatItem.dataset.chatId = chat.id;
            chatItem.innerHTML = `
                <h6>Chat with ${chat.receiverId} ; ${chat.senderId}</h6>
                <p>${chat.lastMessage ? chat.lastMessage.content : 'No messages yet'}</p>
                <span class="badge bg-primary">Unreads: ${chat.unreadCount ? 5 : 0}</span>
            `;
            chatItem.addEventListener('click', () => {selectChat(chatItem, chat.id);
            localStorage.setItem("currentChat", JSON.stringify(chat))});
            chatListElement.appendChild(chatItem);
        });
    } catch (error) {
        console.error('Error loading chats:', error);
        showAlert('Failed to load chats', 'danger');
    }
}

async function selectChat(chatItem, chatId) {
    document.querySelectorAll('.chat-item').forEach(item => item.classList.remove('selected'));
    chatItem.classList.add('selected');
    const selectedChat = document.getElementById('messageInputBody');
    selectedChat.innerHTML = `
        <input type="text" id="messageInput" class="form-control" placeholder="Type a message">
        <button class="btn btn-primary" onclick="sendMessage()">Send</button>`

    let currentChat = JSON.parse(document.getElementById("currentChat"))
    if(currentChat){
        leaveChat(currentChat.id.toString());
    }

    await loadMessages(chatId);
    await joinChat(chatId.toString());
}

// Function to load messages for a chat
async function loadMessages(chatId) {
    const messageListElement = document.getElementById('messageList');
    messageListElement.innerHTML = ''; // Clear existing messages

    try {
        // Fetch messages for the selected chat from API
        const messages = await makeAuthRequest(`chat/messages/${chatId}`, 'GET');

        if (!messages) {
            showAlert('Failed to load messages', 'danger');
            return;
        }

        // Iterate over each message and create message elements
        messages.forEach(message => {
            const messageElement = document.createElement('div');
            messageElement.className = `message ${message.senderId.toString() === localStorage.getItem("currentProfile") ? 'sender' : 'receiver'}`;
            messageElement.innerHTML = `
                <p>${message.content}</p>
                <small>${new Date(message.sentAt).toLocaleTimeString()}</small>
            `;
            messageListElement.appendChild(messageElement);
        });
        messageListElement.scrollTop = messageListElement.scrollHeight;
    } catch (error) {
        console.error('Error loading messages:', error);
        showAlert('Failed to load messages', 'danger');
    }
}

function scrollToBottom() {
    const messageListElement = document.getElementById('messageList');
    messageListElement.scrollTop = messageListElement.scrollHeight;
}


const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5094/chatHub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => {
            const token = localStorage.getItem('accessToken');
            if (!token) {
                console.error("No access token found in local storage");
                return null;
            }
            return token;
        }
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Start the connection
async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.error(err.toString());
        console.error("Error on creating connection");
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

// Receive messages
connection.on("ReceiveMessage", function (senderId, message) {
    const msg = document.createElement("div");
    msg.className = `message ${senderId === localStorage.getItem("currentProfile") ? 'sender' : 'receiver'}`;
    msg.innerHTML = `
                <p>${message}</p>
                <small>${new Date().toLocaleTimeString()}</small>
            `;
    document.getElementById("messageList").appendChild(msg);
    scrollToBottom();
});

async function sendMessage() {
    const messageInput = document.getElementById("messageInput").value;
    const chat = JSON.parse(localStorage.getItem("currentChat"));
    const chatId = chat.id.toString();
    const senderId = localStorage.getItem("currentProfile");
    const receiverId = senderId === chat.senderId.toString() ? chat.receiverId.toString() : chat.senderId.toString();
    console.log("sender:" + senderId);
    console.log("receiver:" + receiverId);
    try {
        await connection.invoke("SendMessage", chatId, senderId, receiverId, messageInput);
        document.getElementById("messageInput").value = '';
    } catch (err) {
        console.error("Send message error:", err.toString());
    }
}