document.addEventListener('DOMContentLoaded', checkAuthForPage);

function isAuthenticated() {
    return localStorage.getItem('isAuthenticated') === 'true';
}

function requireAuth() {
    if (!isAuthenticated()) {
        window.location.href = 'login.html';
    }
}
function checkAuthForPage() {
    if (!isAuthenticated() && !window.location.href.includes('login.html') && !window.location.href.includes('register.html')) {
        window.location.href = 'login.html';
    }
}
// Function to edit profile
function editProfile(profileId) {
    localStorage.setItem('currentProfile', JSON.stringify(profileId));
    window.location.href = `edit-profile.html`;
}

// Function to remove profile
function removeProfile() {
    const profileId = localStorage.getItem('currentProfileId');
    let profiles = JSON.parse(localStorage.getItem('profiles'));
    profiles = profiles.filter(p => p.profileId !== profileId);
    localStorage.setItem('profiles', JSON.stringify(profiles));
    alert('Profile removed successfully!');
    window.location.href = 'user.html';
}

// Function to view profile
function viewProfile(profileId) {
    localStorage.setItem('currentProfile', JSON.stringify(profileId));
    window.location.href = 'view-profile.html';
}


function viewMembership() {
    window.location.href = 'membership.html';
}


function loadContent(file) {
    fetch(file)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            document.getElementById('content').innerHTML = data;
        })
        .catch(error => {
            document.getElementById('content').innerHTML = '<p>Error loading content.</p>';
            console.error('There was a problem with the fetch operation:', error);
        });
}
