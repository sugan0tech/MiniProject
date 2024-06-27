document.addEventListener('DOMContentLoaded', async () => {
    await loadProfiles();
});

async function loadProfiles() {
    try {
        const profiles = await makeAuthRequest('Profile');
        displayProfiles(profiles);
    } catch (error) {
        console.error('Failed to load profiles:', error);
    }
}

function displayProfiles(profiles) {
    const profileList = document.getElementById('profile-list');
    profileList.innerHTML = profiles.map(profile => `
        <div class="card mb-3">
            <div class="card-body">
                <h5 class="card-title">Profile ID: ${profile.profileId}</h5>
                <p class="card-text">User Name: ${profile.user.firstName} ${profile.user.lastName}</p>
                <p class="card-text">Membership ID: ${profile.membershipId}</p>
                <p class="card-text">Membership Type: ${profile.membership.type}</p>
                <button class="btn btn-primary" onclick="openMembershipPopup('${encodeURI(JSON.stringify(profile.membership))}')">Update Membership</button>
            </div>
        </div>
    `).join('');
}

// Function to display current membership details in the modal
function displayCurrentMembership(membership) {
    const membershipType = document.getElementById('membershipType');
    const membershipEndsAt = document.getElementById('membershipEndsAt');
    const membershipUsage = document.getElementById('membershipUsage');

    membershipType.querySelector('span').textContent = membership.type || 'N/A';
    membershipEndsAt.querySelector('span').textContent = membership.endsAt || 'N/A';
    membershipUsage.querySelector('span').textContent = `${membership.viewsCount} views, ${membership.chatCount} chats, ${membership.requestCount} requests, ${membership.viewersViewCount} Viewers views`;

    document.querySelector('.col-md-auto:nth-child(1) .btn').classList.replace('btn-secondary', 'btn-primary');
    document.querySelector('.col-md-auto:nth-child(1) .btn').textContent = 'Select';
    document.querySelector('.col-md-auto:nth-child(1) .btn').disabled = false;
    document.querySelector('.col-md-auto:nth-child(2) .btn').classList.replace('btn-secondary', 'btn-primary');
    document.querySelector('.col-md-auto:nth-child(2) .btn').textContent = 'Select';
    document.querySelector('.col-md-auto:nth-child(2) .btn').disabled = false;
    document.querySelector('.col-md-auto:nth-child(3) .btn').classList.replace('btn-secondary', 'btn-primary');
    document.querySelector('.col-md-auto:nth-child(3) .btn').textContent = 'Select';
    document.querySelector('.col-md-auto:nth-child(3) .btn').disabled = false;

    if (membership.type === 'BasicUser') {
        document.querySelector('.col-md-auto:nth-child(2) .btn').classList.replace('btn-primary', 'btn-secondary');
        document.querySelector('.col-md-auto:nth-child(2) .btn').textContent = 'Current Plan';
        document.querySelector('.col-md-auto:nth-child(2) .btn').disabled = true;
    } else if (membership.type === 'PremiumUser') {
        document.querySelector('.col-md-auto:nth-child(3) .btn').classList.replace('btn-primary', 'btn-secondary');
        document.querySelector('.col-md-auto:nth-child(3) .btn').textContent = 'Current Plan';
        document.querySelector('.col-md-auto:nth-child(3) .btn').disabled = true;
    } else {
        document.querySelector('.col-md-auto:nth-child(1) .btn').classList.replace('btn-primary', 'btn-secondary');
        document.querySelector('.col-md-auto:nth-child(1) .btn').textContent = 'Current Plan';
        document.querySelector('.col-md-auto:nth-child(1) .btn').disabled = true;
    }
}

// Function to open the membership selection popup and populate with current membership details
function openMembershipPopup(encodedMembership) {
    // Getting membership from string
    let membership = JSON.parse(decodeURI(encodedMembership))

    // Show the modal
    var myModal = new bootstrap.Modal(document.getElementById('membershipSelectionModal'), {
        keyboard: false
    });
    myModal.show();
    displayCurrentMembership(membership);

    document.getElementById('profileIdInput').value = membership.profileId;
    document.getElementById('currentMembershipId').value = membership.id;
    document.getElementById('profileIdInput').value = membership.profileId;
}

// Function to handle membership selection
async function selectMembership(membershipType) {
    const profileId = document.getElementById('profileIdInput').value;
    const membershipId = document.getElementById('currentMembershipId').value; // Use current membership ID

    // Close the modal
    var myModal = bootstrap.Modal.getInstance(document.getElementById('membershipSelectionModal'));
    myModal.hide();

    // Perform API call to update membership
    await updateMembership(profileId, membershipId, membershipType);
}

// Function to update membership via API call
async function updateMembership(profileId, membershipId, membershipType) {
    var membership = makeAuthRequest('Membership/profile/' + profileId )
    var data = await membership
    data.type = membershipType
    data.description = "Updated By Admins"
    console.log(data)
    try {
        const response = await makeAuthRequest(`Membership`,
            'PUT',
            data
        );

        showAlert('Membership updated successfully.', 'success');
        await loadProfiles(); // Refresh profiles after updating membership
    } catch (error) {
        console.error('Error updating membership:', error);
        showAlert('Failed to update membership. Please try again later.', 'danger');
    }
}
