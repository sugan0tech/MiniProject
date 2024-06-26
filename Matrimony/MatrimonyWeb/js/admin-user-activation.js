document.addEventListener('DOMContentLoaded', async () => {
    await loadUserActivationData();

    document.getElementById('activate-user-btn').addEventListener('click', async () => {
        const userId = document.getElementById('user-id-input').value;
        await updateUserActivation(userId, true);
    });

    document.getElementById('deactivate-user-btn').addEventListener('click', async () => {
        const userId = document.getElementById('user-id-input').value;
        await updateUserActivation(userId, false);
    });
});

async function loadUserActivationData() {
    try {
        const users = await makeAuthRequest('User/all');
        displayUsers(users);
    } catch (error) {
        console.error('Failed to load user data:', error);
    }
}

function displayUsers(users) {
    const verifiedUsers = users.filter(user => user.isVerified);
    const unverifiedUsers = users.filter(user => !user.isVerified);

    const verifiedUsersList = document.getElementById('verified-users-list');
    const unverifiedUsersList = document.getElementById('unverified-users-list');

    verifiedUsersList.innerHTML = verifiedUsers.map(user => `<li>Id: ${user.userId} Mail: ${user.email} </li>`).join('');
    unverifiedUsersList.innerHTML = unverifiedUsers.map(user => `<li>Id: ${user.userId} Mail: ${user.email} Id: ${user.userId}</li>`).join('');
}

async function updateUserActivation(userId, isVerified) {
    try {
        const payload = { userId, isVerified };
        await makeAuthRequest(`User/validate/${userId}/${isVerified}`, 'POST');
        showAlert('User status updated successfully.', 'success');
        await loadUserActivationData()
    } catch (error) {
        showAlert('Failed to update user status', 'danger');
        console.error('Failed to update user status:', error);
    }
}
