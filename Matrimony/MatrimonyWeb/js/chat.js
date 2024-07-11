// Function to load chats
async function loadChats() {
    await refreshAccessToken()
    const chatListElement = document.getElementById('chatList');
    chatListElement.innerHTML = ''; // Clear existing chat list

    const currentChat = JSON.parse(localStorage.getItem("currentChat"));
    const currentProfileId = localStorage.getItem("currentProfile");
    const currentProfile = JSON.parse(localStorage.getItem(`profile${currentProfileId}`))
    const membershipType = currentProfile.membership.type

    try {
        const chats = await makeAuthRequest(`chat/chats/${currentProfileId}`, 'GET');

        if (!chats || chats.length === 0) {
            showAlert("No messages found, start one from match", 'warning');
            return;
        }

        // Iterate over each chat and create chat elements
        for (const chat of chats) {
            // Determine the opposite profile ID
            const oppositeProfileId = chat.receiverId == currentProfileId ? chat.senderId : chat.receiverId;

            // Fetch profile details for the opposite profile
            const oppositeProfile = await makeAuthRequest(`profile/preview/${oppositeProfileId}`, 'GET');

            // Create chat item element
            const chatItem = document.createElement('div');
            chatItem.className = 'chat-item';
            chatItem.dataset.chatId = chat.id;

            // Get profile picture or use a placeholder if not available
            const profilePictureSrc = oppositeProfile.profilePicture
                ? `data:image/jpeg;base64,${oppositeProfile.profilePicture}`
                : './placeholder.jpg';

            const messages = await makeAuthRequest(`chat/messages/${chat.id}`, 'GET');
            const lastMessage = messages.pop();
            console.log(lastMessage)
            chatItem.innerHTML = `
                <div class="d-flex align-items-center">
                    <img src="${profilePictureSrc}" alt="Profile Picture" class="img-thumbnail mr-3" style="width: 50px; height: 50px;">
                    <div>
                        <h6>${oppositeProfile.user.firstName} ${oppositeProfile.user.lastName}</h6>
                        <h6>Chat between ${chat.receiverId} ; ${chat.senderId}</h6>
                        <p>Last message: ${lastMessage ? lastMessage.content : 'No messages yet'}</p>
                        <span class="badge bg-primary">Unreads: ${chat.unreads || 0}</span>
                    </div>
                </div>
            `;

            if (membershipType == 'PremiumUser') {
                chatItem.addEventListener('click', () => {
                    selectChat(chatItem, chat.id);
                    localStorage.setItem("currentChat", JSON.stringify(chat));
                });

                // Auto-select current chat if it matches the stored current chat
                if (currentChat && chat.id === currentChat.id) {
                    selectChat(chatItem, chat.id);
                }
            } else {
                chatItem.addEventListener('click', () => {
                    showAlert('Upgrade to Premium view messages & chat', 'warning');
                });
            }


            chatListElement.appendChild(chatItem);
        }
    } catch (error) {
        console.error('Error loading chats:', error);
        showAlert('Failed to load chats', 'danger');
    }
}

async function selectChat(chatItem, chatId) {
    if (window.innerWidth <= 768)
        toggleChatList()
    document.querySelectorAll('.chat-item').forEach(item => item.classList.remove('selected'));
    chatItem.classList.add('selected');

    const selectedChat = document.getElementById('messageInputBody');
    selectedChat.innerHTML = `
        <button class="btn btn-info mt-1" onclick="viewProfileButtonClick('${chatId}')" style="margin-right: 10px">View Profile</button>
        <input type="text" id="messageInput" class="form-control" placeholder="Type a message">
        <button class="btn btn-primary mt-1" onclick="sendMessage()">Send</button>`;

    let currentChat = JSON.parse(localStorage.getItem("currentChat"));
    if (currentChat) {
        leaveChat(currentChat.id.toString());
    }

    await loadMessages(chatId);
    await joinChat(chatId.toString());
}

async function viewProfileButtonClick(chatId) {
    const currentProfileId = localStorage.getItem("currentProfile");
    const chat = JSON.parse(localStorage.getItem("currentChat"));

    if (!chat) {
        showAlert('Chat not found', 'warning');
        return;
    }

    const oppositeProfileId = chat.receiverId == currentProfileId ? chat.senderId : chat.receiverId;

    await viewProfileViaChat(oppositeProfileId);
}


async function viewProfileViaChat(profileId) {
    const currentProfileId = localStorage.getItem('currentProfile');
    const endpoint = `ProfileView/add/viewer/${currentProfileId}/profile/${profileId}`;

    try {
        const profileDetails = await makeAuthRequest(endpoint, 'POST');

        if (profileDetails) {
            displayProfileModal(profileDetails);
        } else {
            console.log("Profile not found.");
        }
    } catch (error) {
        console.error('Error viewing profile:', error);
    }
}

function displayProfileModal(profileDetails) {
    const existingModal = document.getElementById('profileModal');
    if (existingModal) {
        existingModal.remove();
    }
    const modalHtml = `
        <div class="modal fade" id="profileModal" tabindex="-1" aria-labelledby="profileModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="profileModalLabel">Profile Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        ${profileDetails.profilePicture ? `<img src="${profileDetails.profilePicture}" alt="Profile Picture" class="img-thumbnail mb-3" style=""/>` : ''}
                        <h6>${profileDetails.user.firstName} ${profileDetails.user.lastName}</h6>
                        <p><strong>Profile ID:</strong> ${profileDetails.profileId}</p>
                        <p><strong>Occupation:</strong> ${profileDetails.occupation}</p>
                        <p><strong>Marital Status:</strong> ${profileDetails.maritalStatus}</p>
                        <p><strong>Mother Tongue:</strong> ${profileDetails.motherTongue}</p>
                        <p><strong>Religion:</strong> ${profileDetails.religion}</p>
                        <p><strong>Ethnicity:</strong> ${profileDetails.ethnicity}</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    const modalContainer = document.createElement('div');
    modalContainer.innerHTML = modalHtml;
    document.body.appendChild(modalContainer);

    const modal = new bootstrap.Modal(document.getElementById('profileModal'));
    modal.show();
}

function sanitizeHTML(str) {
    const tempDiv = document.createElement('div');
    tempDiv.textContent = str;
    return tempDiv.innerHTML;
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
                <p>${sanitizeHTML(message.content)}</p>
                <small>${new Date(message.sentAt).toLocaleTimeString()}</small>
            `;
            messageListElement.appendChild(messageElement);
            if (message.senderId.toString() !== localStorage.getItem("currentProfile")) {
                // Call SeenMessage via SignalR
                connection.invoke("SeenMessage", chatId.toString(), message.id.toString());
            }
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
function toggleChatList() {
    const chatList = document.getElementById('chatList');
    let chatWindow = document.getElementsByClassName("chat-window").item(0)
    if (chatWindow){
        if (chatWindow.classList.contains('d-none')){
            chatWindow.classList.remove('d-none');
            chatList.classList.add('d-none');
        }else {
            chatWindow.classList.add('d-none');
            chatList.classList.remove('d-none');
        }
    }
}
