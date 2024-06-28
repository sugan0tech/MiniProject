const currentProfileId = JSON.parse(localStorage.getItem('currentProfile'));

// Fetch and display sent match requests
async function fetchSentMatches() {
    try {
        const sentMatches = await makeAuthRequest(`MatchRequest/sent/${currentProfileId}`);
        if (sentMatches) {
            displayMatches(sentMatches, 'sentMatches', 'sent');
        }
    } catch (error) {
        console.error('Error fetching sent matches:', error);
    }
}

// Fetch and display accepted match requests
async function fetchAcceptedMatches() {
    try {
        const acceptedMatches = await makeAuthRequest(`MatchRequest/accepted/${currentProfileId}`);
        if (acceptedMatches) {
            displayMatches(acceptedMatches, 'acceptedMatches', 'accepted');
        }
    } catch (error) {
        console.error('Error fetching accepted matches:', error);
    }
}

// Fetch and display received match requests
async function fetchReceivedMatches() {
    try {
        const receivedMatches = await makeAuthRequest(`MatchRequest/received/${currentProfileId}`);
        if (receivedMatches && receivedMatches.length > 0) {
            document.getElementById('receivedNotification').style.display = 'inline';
        }
        if (receivedMatches) {
            displayMatches(receivedMatches, 'receivedMatches', 'received');
        }
    } catch (error) {
        console.error('Error fetching received matches:', error);
    }
}

// Display matches in the specified container
function displayMatches(matches, containerId, type) {
    const container = document.getElementById(containerId);
    container.innerHTML = '';

    matches.forEach(match => {
        const matchCard = `
            <div class="card mb-3 ${match.isRejected ? 'border-danger' : match.receiverLike ? 'border-success' : ''}" data-match-id="${match.matchId}">
                <div class="card-body">
                    <h5 class="card-title">Match ${match.matchId}: Profile ${match.receivedProfileId}</h5>
                    <p class="card-text">Sent by Profile ${match.sentProfileId}</p>
                    ${type === 'received' ? `
                        <button class="btn btn-success" onclick="acceptMatch(${match.matchId}, ${match.receivedProfileId})" style="">Accept Match</button>
                        <button class="btn btn-danger" onclick="rejectMatch(${match.matchId}, ${match.receivedProfileId})">Reject Match</button>
                        <button class="btn btn-primary" onclick="viewProfile(${match.sentProfileId})">View Profile</button>
                    ` : type === 'accepted' ? `
                        <button class="btn btn-primary" onclick="openChat(${match.sentProfileId}, ${match.receivedProfileId})">Chat</button>
                    `: `
                        <button class="btn btn-danger" onclick="removeMatch('${type}', ${match.matchId})">Remove Match</button>
                    `}
                </div>
            </div>
        `;
        container.innerHTML += matchCard;
    });
}

// Accept a match request
async function acceptMatch(matchId, profileId) {
    try {
        const response = await makeAuthRequest(`MatchRequest/approve/${matchId}/${profileId}`, 'POST');
        if (response) {
            alert('Match accepted successfully!');
            await fetchReceivedMatches(); // Refresh the list
        }
    } catch (error) {
        console.error('Error accepting match:', error);
    }
}

// Reject a match request
async function rejectMatch(matchId, profileId) {
    try {
        const response = await makeAuthRequest(`MatchRequest/reject/${matchId}/${profileId}`, 'POST');
        if (response) {
            alert('Match rejected successfully!');
            await fetchReceivedMatches(); // Refresh the list
        }
    } catch (error) {
        console.error('Error rejecting match:', error);
    }
}

// Remove a match from the list
async function removeMatch(type, matchId) {
    const containerId = type === 'sent' ? 'sentMatches' : 'acceptedMatches';
    const container = document.getElementById(containerId);
    const matchCard = container.querySelector(`[data-match-id="${matchId}"]`);
    if (matchCard) {
        matchCard.remove();
    }
    await makeAuthRequest('MatchRequest/' + matchId, 'DELETE');
}

async function openChat(sentProfileId, receivedProfileId) {
    try {
        // Check if a chat already exists for these profiles
        const existingChat = await makeAuthRequest(`Chat/findChat/${sentProfileId}/${receivedProfileId}`);

        if (existingChat && existingChat.id) {
            // Chat exists, open the existing chat
            localStorage.setItem('currentChat', JSON.stringify(existingChat));
            window.location.href = 'chats.html'
        } else {
            // No chat exists, create a new chat
            const newChat = await makeAuthRequest(`Chat/${sentProfileId}/${receivedProfileId}`, 'POST');
            if (newChat && newChat.chatId) {
                // New chat created, open it
                localStorage.setItem('currentChat', JSON.stringify(newChat));
                window.location.href = 'chats.html'
            } else {
                showAlert('Failed to create or open chat', 'danger');
            }
        }
    } catch (error) {
        console.error('Error in openChat:', error);
        showAlert('Failed to open chat', 'danger');
    }
}