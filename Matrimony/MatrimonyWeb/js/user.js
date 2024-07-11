// Function to load user account info
async function loadAccountInfo() {
    try {
        const userInfo = await makeAuthRequest('User', 'GET');
        document.getElementById('name').value = `${userInfo.firstName} ${userInfo.lastName}`;
        document.getElementById('phone').value = userInfo.phoneNumber;
        document.getElementById('email').value = userInfo.email;
        document.getElementById('id').value = userInfo.userId;

        if (!userInfo.addressId || userInfo.addressId == 0){
        }
        else{
            await loadAddressInfo(userInfo.addressId);
        }
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
        userId: document.getElementById('id').value,
        phoneNumber: document.getElementById('phone').value,
        firstName,
        lastName,
        isVerified: true,
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

async function loadAddressInfo(addressId) {
    try {
        const addressInfo = await makeAuthRequest(`Address/${addressId}`, 'GET');
        document.getElementById('street').value = addressInfo.street;
        document.getElementById('addressId').value = addressId;
        document.getElementById('city').value = addressInfo.city;
        document.getElementById('state').value = addressInfo.state;
        document.getElementById('country').value = addressInfo.country;
    } catch (error) {
        showAlert("Failed to load address info. Please try again.", 'danger');
        if (error.message === '404') console.error("Address not found");
    }
}

// update button disable handler, if there is no change
document.addEventListener("DOMContentLoaded", function() {
    const accountInfoForm = document.getElementById("accountInfoForm");
    const addressForm = document.getElementById("addressForm");

    function setOriginalValues(form) {
        const inputs = form.querySelectorAll("input");
        inputs.forEach(input => {
            input.setAttribute("data-original-value", input.value);
        });
    }

    function checkForModifications(form, button) {
        const inputs = form.querySelectorAll("input");
        let isModified = false;
        inputs.forEach(input => {
            if (input.value !== input.getAttribute("data-original-value")) {
                isModified = true;
            }
        });
        button.disabled = !isModified;
    }

    setOriginalValues(accountInfoForm);
    setOriginalValues(addressForm);

    const accountInfoButton = accountInfoForm.querySelector("button[type='submit']");
    const addressButton = addressForm.querySelector("button[type='submit']");

    accountInfoForm.addEventListener("input", function() {
        checkForModifications(accountInfoForm, accountInfoButton);
    });

    addressForm.addEventListener("input", function() {
        checkForModifications(addressForm, addressButton);
    });

    accountInfoButton.disabled = true;
    addressButton.disabled = true;
});

// Function to save or update address info
document.getElementById('addressForm').addEventListener('submit', async function(event) {
    event.preventDefault();
    const userId = document.getElementById('id').value;
    const addressData = {
        userId: userId,
        street: document.getElementById('street').value,
        city: document.getElementById('city').value,
        state: document.getElementById('state').value,
        country: document.getElementById('country').value,
    };

    try {
        // Check if user already has an address
        const userInfo = await makeAuthRequest('User', 'GET');
        if (userInfo.addressId) {
            // Update existing address
            addressData.addressId = userInfo.addressId;
            await makeAuthRequest('Address', 'PUT', addressData);
            showAlert("Address updated successfully.", 'success');
        } else {
            // Add new address
            addressData.userId = userId;
            await makeAuthRequest('Address', 'POST', addressData);
            showAlert("Address added successfully.", 'success');
        }
    } catch (error) {
        showAlert("Failed to save address info. Please try again.", 'danger');
        if (error.message === '401') console.error("unauthorized");
    }
});

// Function to load user profiles
async function loadProfiles() {
    try {
        const profiles = await makeAuthRequest('Profile/manager');

        // Flush existing profile data in localStorage
        const keys = Object.keys(localStorage);
        keys.forEach(key => {
            if (key.includes("profile") && !key.includes("currentProfile")) {
                localStorage.removeItem(key);
            }
        });

        // Set the current profile if not set
        if (!localStorage.getItem("currentProfile")) {
            localStorage.setItem("currentProfile", profiles[0].profileId);
        }

        const profilesList = document.getElementById('profilesList');
        profilesList.innerHTML = '';

        profiles.forEach(profile => {
            localStorage.setItem(`profile${profile.profileId}`, JSON.stringify(profile));

            const profilePictureSrc = profile.profilePicture
                ? `${profile.profilePicture}`
                : './placeholder.jpg';

            const profileHTML = `
                <div class="card mb-3" data-profile-id="${profile.profileId}">
                    <div class="card-body d-flex align-items-start">
                        <img src="${profilePictureSrc}" alt="Profile Picture" class="img-thumbnail me-3" style="max-width: 150px;">
                        <div>
                            <h5 class="card-title">Profile ${profile.profileId}</h5>
                            <p class="card-text">Name: ${profile.user.firstName} ${profile.user.lastName}, IsVerified: ${profile.user.isVerified}</p>
                            <p class="card-text">Age: ${profile.age}, MaritalStatus: ${profile.maritalStatus}</p>
                            <p class="card-text">Education: ${profile.education}, Occupation: ${profile.occupation}</p>
                            <p class="card-text"><strong>Membership:</strong> ${profile.membership ? profile.membership.type : "FreeUser"}</p>
                            <button class="btn btn-info" onclick="viewUserProfile(${profile.profileId})">View Profile</button>
                            <button class="btn btn-warning" onclick="editProfile(${profile.profileId})">Edit Profile</button>
                            <button class="btn btn-danger" onclick="removeProfile(${profile.profileId})">Remove Profile</button>
                            <button class="btn btn-primary" onclick="viewMembershipNew(${profile.profileId})">View Membership</button>
                        </div>
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
        await refreshAccessToken()
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

function contactAdmin() {
    // Implement the logic to contact admin here
    showAlert("Admin contact feature is not implemented yet", 'info');
}
