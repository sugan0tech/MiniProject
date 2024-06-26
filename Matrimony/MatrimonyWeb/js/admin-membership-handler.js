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
                <button class="btn btn-primary" onclick="openMembershipPopup(${JSON.stringify(profile.membership)})">Update Membership</button>
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
    membershipUsage.querySelector('span').textContent = `${membership.viewsCount} views, ${membership.chatCount} chats`;
}

// Function to open the membership selection popup and populate with current membership details
function openMembershipPopup(membership) {
    // Display current membership details in the modal
    membership = JSON.parse(membership)
    displayCurrentMembership(membership);

    // Show the modal
    var myModal = new bootstrap.Modal(document.getElementById('membershipSelectionModal'), {
        keyboard: false
    });
    myModal.show();

    // Update the hidden input field with the current profileId
    document.getElementById('profileIdInput').value = membership.profileId;
}

// Function to handle membership selection
async function selectMembership(membershipType, membershipId) {
    const profileId = document.getElementById('profileIdInput').value;

    // Close the modal
    var myModal = bootstrap.Modal.getInstance(document.getElementById('membershipSelectionModal'));
    myModal.hide();

    // Perform API call to update membership
    await updateMembership(profileId, membershipId, membershipType);
}

// Function to update membership via API call
async function updateMembership(profileId, membershipId, membershipType) {
    const url = `http://localhost:5094/api/Membership/${membershipId}/updateMembership`;
    const data = {
        profileId: profileId,
        type: membershipType,
        isTrail: false,
        isTrailEnded: false
    };

    try {
        const response = await fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            const errorMessage = await response.json();
            throw new Error(errorMessage.message || 'Failed to update membership.');
        }

        // Handle success as needed (e.g., show a success message, update UI)
        showAlert('Membership updated successfully.', 'success');
        await loadProfiles(); // Refresh profiles after updating membership
    } catch (error) {
        console.error('Error updating membership:', error);
        showAlert('Failed to update membership. Please try again later.', 'danger');
    }
}

// Helper function to display alerts
function showAlert(message, type) {
    const alertContainer = document.getElementById('alert-container');
    const alert = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;
    alertContainer.innerHTML = alert;
}
