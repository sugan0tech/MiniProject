// Function to load user account info
async function loadAccountInfo() {
    try {
        const userInfo = await makeAuthRequest('User', 'GET');
        document.getElementById('name').value = `${userInfo.firstName} ${userInfo.lastName}`;
        document.getElementById('email').value = userInfo.email;
    } catch (error) {
        showAlert("Failed to load account info. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
}

// Function to update user account info
async function updateAccountInfo(event) {
    event.preventDefault();
    const [firstName, lastName] = document.getElementById('name').value.split(' ');
    const data = {
        firstName,
        lastName,
        email: document.getElementById('email').value
    };

    try {
        await makeAuthRequest('User', "PUT", data);
        showAlert("Account info updated successfully.", 'success');
    } catch (error) {
        showAlert("Failed to update account info. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
}

// Function to load user profiles
async function loadProfiles() {
    try {
        const profiles = await makeAuthRequest('Profile/manager');
        console.log("stuff")
        // flushes existing profiles dummy ones
        const keys = Object.keys(localStorage)
        let range = keys.length;
        while ( range-- ){
            if (keys[range].includes("profile") && !keys[range].includes("currentProfile"))
                localStorage.removeItem(keys[range])
        }

        const profilesList = document.getElementById('profilesList');
        profilesList.innerHTML = '';
        profiles.forEach(profile => {
            localStorage.setItem(`profile${profile.profileId}`, JSON.stringify(profile));
            const profileHTML = `
                <div class="card mb-3" data-profile-id="${profile.profileId}">
                    <div class="card-body">
                        <h5 class="card-title">Profile ${profile.profileId}</h5>
                        <p class="card-text">Name: ${profile.user.firstName + " " + profile.user.lastName}, IsVerified: ${profile.user.isVerified}</p>
                        <p class="card-text">Age: ${profile.age}, Location: ${profile.location}</p>
                        <p class="card-text">Education: ${profile.education}, Occupation: ${profile.occupation}</p>
                        <p class="card-text"><strong>Membership:</strong> ${profile.membership? profile.membership.type : "FreeUser"}</p>
                        <button class="btn btn-info" onclick="viewUserProfile(${profile.profileId})">View Profile</button>
                        <button class="btn btn-warning" onclick="editProfile(${profile.profileId})">Edit Profile</button>
                        <button class="btn btn-danger" onclick="removeProfile(${profile.profileId})">Remove Profile</button>
                        <button class="btn btn-primary" onclick="viewMembership(${profile.profileId})">View Membership</button>
                    </div>
                </div>
            `;
            profilesList.insertAdjacentHTML('beforeend', profileHTML);
        });
    } catch (error) {
        showAlert("Failed to load profiles. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
}

async function loadLoggedDevices() {
    try {
        const devices = await makeAuthRequest('UserSession/user');
        const devicesList = document.getElementById('loggedDevicesList');
        devicesList.innerHTML = '';
        devices.filter(device => device.isValid).forEach(device => {
            const deviceHTML = `
                        <div class="card mb-3" data-device-id="${device.sessionId}">
                            <div class="card-body">
                                <p class="card-text"><strong>Device:</strong> ${device.userAgent}</p>
                                <p class="card-text"><strong>Type:</strong> ${device.deviceType}</p>
                                <p class="card-text"><strong>Loged At:</strong> ${new Date(device.createdAt).toLocaleString()}</p>
                                <button class="btn btn-danger" onclick="removeDevice(${device.sessionId})">Remove Device</button>
                            </div>
                        </div>
                    `;
            devicesList.insertAdjacentHTML('beforeend', deviceHTML);
        });
    } catch (error) {
        showAlert("Failed to load devices. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
}

// Function to remove a device
async function removeDevice(deviceId) {
    try {
        await makeAuthRequest(`UserSession/${deviceId}`, 'DELETE');
        showAlert("Device removed successfully.", 'success');
        await loadLoggedDevices(); // Refresh the device list
    } catch (error) {
        showAlert("Failed to remove device. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
}

// Function to handle password reset
async function resetPassword(event) {
    event.preventDefault();
    const data = {
        password: document.getElementById('currentPassword').value,
        newPassword: document.getElementById('newPassword').value,
        confirmNewPassword: document.getElementById('confirmNewPassword').value
    };

    if (data.newPassword !== data.confirmNewPassword) {
        showAlert("New passwords do not match. Please try again.", 'danger');
        return;
    }

    try {
        await makeAuthRequest('User/ResetPassword', "POST", data);
        showAlert("Password reset successfully.", 'success');
    } catch (error) {
        showAlert("Failed to reset password. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
}

// Function to initialize event listeners and load initial data
async function initialize() {
    document.getElementById('accountInfoForm').addEventListener('submit', updateAccountInfo);
    document.getElementById('resetPasswordForm').addEventListener('submit', resetPassword);
    document.getElementById('createProfileBtn').addEventListener('click', () => {
        window.location.href = 'create-profile.html';
    });
    for (let i = 0; i < 1000; i++) {
        console.log("mass")
    }
    await loadAccountInfo();
    await loadProfiles();
    await loadLoggedDevices();
}

document.addEventListener('DOMContentLoaded', initialize);
