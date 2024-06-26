document.addEventListener('DOMContentLoaded', async () => {
    await loadUserSessions();

    document.getElementById('invalidate-sessions-btn').addEventListener('click', async () => {
        const userId = document.getElementById('user-id-input').value;
        await invalidateUserSessions(userId);
    });
});

async function loadUserSessions() {
    try {
        const sessions = await makeAuthRequest('UserSession');
        displaySessions(sessions.filter(session => session.isValid));
    } catch (error) {
        console.error('Failed to load user sessions:', error);
    }
}

function displaySessions(sessions) {
    const sessionList = document.getElementById('session-list');
    sessionList.innerHTML = '';

    // Create an object to group sessions by userId
    const groupedSessions = sessions.reduce((acc, session) => {
        if (!acc[session.userId]) {
            acc[session.userId] = [];
        }
        acc[session.userId].push(session);
        return acc;
    }, {});

    // Iterate over grouped sessions and create pallets
    for (const userId in groupedSessions) {
        const sessionsForUser = groupedSessions[userId];
        const count = sessionsForUser.length;

        const palletHTML = `
            <div class="card mb-3">
                <div class="card-body">
                    <h5 class="card-title">User ID: ${userId} - Sessions: ${count}</h5>
                    <ul class="list-group list-group-flush">
                        ${sessionsForUser.map(session => `
                            <li class="list-group-item">Session ID: ${session.sessionId}</li>
                        `).join('')}
                    </ul>
                    <button class="btn btn-danger mt-3" onclick="invalidateUserSessions(${userId})">Invalidate User</button>
                </div>
            </div>
        `;

        sessionList.innerHTML += palletHTML;
    }
}

async function invalidateUserSessions(userId) {
    try {
        await makeAuthRequest(`UserSession/invalidateAll/${userId}`, 'POST');
        showAlert('User sessions invalidated successfully.', 'success');
        await loadUserSessions();
    } catch (error) {
        console.error('Failed to invalidate user sessions:', error);
        showAlert('Failed to invalidate user sessions:', 'danger');
    }
}
