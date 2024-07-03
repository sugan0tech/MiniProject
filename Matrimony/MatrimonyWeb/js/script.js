document.addEventListener('DOMContentLoaded', checkAuthForPage);

function isAuthenticated() {
    return localStorage.getItem('isAuthenticated') === 'true';
}

function requireAuth() {
    if (!isAuthenticated()) {
        window.location.href = 'login.html';
    }
}
requireAuth();
function checkAuthForPage() {
    if (!isAuthenticated() && !window.location.href.includes('login.html') && !window.location.href.includes('register.html')) {
        window.location.href = 'login.html';
    }
}
// Function to edit profile
function editProfile(profileId) {
    if(!profileId || localStorage.getItem("currentProfile")){
        window.location.href = `edit-profile.html`;
        return;
    }

    localStorage.setItem("currentProfile", profileId);
    window.location.href = `edit-profile.html`;
}

// Function to remove profile
async function removeProfile(profileId) {
    let profiles = JSON.parse(localStorage.getItem('profiles'));
    await makeAuthRequest("Profile/" + profileId , "DELETE")
    localStorage.removeItem('currentProfile');
    localStorage.removeItem('profile' + profileId);
    window.location.href = 'user.html';
}

// Function to view profile
function viewUserProfile(profileId) {
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

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
