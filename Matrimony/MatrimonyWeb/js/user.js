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

        if (localStorage.getItem("currentProfile") == null){
            localStorage.setItem("currentProfile", profiles[0].profileId)
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
                        <button class="btn btn-primary" onclick="viewMembershipNew(${profile.profileId})">View Membership</button>
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
    let email = parseJwt(localStorage.getItem("accessToken"))["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
    const data = {
        email: email,
        password: document.getElementById('currentPassword').value,
        newPassword: document.getElementById('newPassword').value,
        confirmNewPassword: document.getElementById('confirmNewPassword').value
    };

    if (data.newPassword !== data.confirmNewPassword) {
        showAlert("New passwords do not match. Please try again.", 'danger');
        return;
    }

    try {
        await makeAuthRequest('Auth/ResetPassword', "POST", data);
        showAlert("Password reset successfully. please login again", 'success');
        await sleep(3000)
        await logout()
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

async function viewMembershipNew(currentProfile) {
    if (!currentProfile ) {
        showAlert("No profile selected", 'danger');
        return;
    }

    try {
        const membership = await makeAuthRequest(`Membership/profile/${currentProfile}`);
        if (membership) {
            document.getElementById('membershipType').querySelector('span').textContent = membership.type;
            document.getElementById('membershipEndsAt').querySelector('span').textContent = new Date(membership.endsAt).toLocaleDateString();
            document.getElementById('membershipViewsCount').querySelector('span').textContent = membership.viewsCount;
            document.getElementById('membershipChatCount').querySelector('span').textContent = membership.chatCount;
            document.getElementById('membershipRequestCount').querySelector('span').textContent = membership.requestCount;
            document.getElementById('membershipViewersViewCount').querySelector('span').textContent = membership.viewersViewCount;

            // Hide all buttons initially
            const buttons = ['freeButton', 'basicButton', 'premiumButton'];
            buttons.forEach(btn => {
                document.getElementById(btn).classList.add('d-none');
            });

            // Show and style the current plan button
            let currentButton;
            switch(membership.type.toLowerCase()) {
                case 'free':
                    currentButton = document.getElementById('freeButton');
                    break;
                case 'basic':
                case 'basicuser':
                    currentButton = document.getElementById('basicButton');
                    break;
                case 'premium':
                case 'premiumuser':
                    currentButton = document.getElementById('premiumButton');
                    break;
            }

            if (currentButton) {
                currentButton.classList.remove('d-none');
                currentButton.classList.replace('btn-primary', 'btn-secondary');
                currentButton.textContent = 'Current Plan';
                currentButton.disabled = true;
            }

            let membershipModal = new bootstrap.Modal(document.getElementById('membershipModal'));
            membershipModal.show();
        } else {
            showAlert("Failed to fetch membership details", 'danger');
        }
    } catch (error) {
        console.error('Error fetching membership:', error);
        showAlert("An error occurred while fetching membership details", 'danger');
    }
}

function contactAdmin() {
    // Implement the logic to contact admin here
    showAlert("Admin contact feature is not implemented yet", 'info');
}
