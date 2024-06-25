document.addEventListener('DOMContentLoaded', () => {
    const currentMembership = {
        type: 'Free',
        endsAt: '2024-12-31',
        usage: '5 matches used out of 5'
    };

    document.getElementById('membershipType').querySelector('span').textContent = currentMembership.type;
    document.getElementById('membershipEndsAt').querySelector('span').textContent = currentMembership.endsAt;
    document.getElementById('membershipUsage').querySelector('span').textContent = currentMembership.usage;

    if (currentMembership.type === 'Basic') {
        document.querySelector('.col-md-4:nth-child(2) .btn').classList.replace('btn-primary', 'btn-secondary');
        document.querySelector('.col-md-4:nth-child(2) .btn').textContent = 'Current Plan';
        document.querySelector('.col-md-4:nth-child(2) .btn').disabled = true;
    } else if (currentMembership.type === 'Premium') {
        document.querySelector('.col-md-4:nth-child(3) .btn').classList.replace('btn-primary', 'btn-secondary');
        document.querySelector('.col-md-4:nth-child(3) .btn').textContent = 'Current Plan';
        document.querySelector('.col-md-4:nth-child(3) .btn').disabled = true;
    } else {
        document.querySelector('.col-md-4:nth-child(1) .btn').classList.add('btn-secondary');
    }
});

let currentProfileId = null;

function viewMembership(profileId) {
    currentProfileId = profileId;

    // Fetch the current membership status (this will be static for now)
    let membershipStatus = document.getElementById(`membershipStatus${profileId}`).textContent;

    // Update modal content based on current membership status
    let membershipStatusContainer = document.getElementById('membershipStatusContainer');
    membershipStatusContainer.innerHTML = `<p><strong>Current Membership:</strong> ${membershipStatus}</p>`;

    // Show the modal
    let membershipModal = new bootstrap.Modal(document.getElementById('membershipModal'));
    membershipModal.show();
}

function selectMembership(type, profileId) {
    // Update the membership status (here you would call your API)
    document.getElementById(`membershipStatus${profileId}`).textContent = type;

    let membershipModal = bootstrap.Modal.getInstance(document.getElementById('membershipModal'));
    membershipModal.hide();

    alert(`Profile ${profileId} upgraded to ${type} Membership`);
}
