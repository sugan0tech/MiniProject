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

// Function to load profile details
function loadProfileDetails(profileId) {
    const profile = JSON.parse(localStorage.getItem('profiles')).find(p => p.profileId === profileId);
    if (profile) {
        const profileDetails = `
            <div class="card mb-3">
                <div class="card-body">
                    <h5 class="card-title">Profile ID: ${profile.profileId}</h5>
                    <p class="card-text">Date of Birth: ${profile.dateOfBirth}</p>
                    <p class="card-text">Age: ${profile.age}</p>
                    <p class="card-text">Education: ${profile.education}</p>
                    <p class="card-text">Annual Income: ${profile.annualIncome}</p>
                    <p class="card-text">Occupation: ${profile.occupation}</p>
                    <p class="card-text">Marital Status: ${profile.maritalStatus}</p>
                    <p class="card-text">Mother Tongue: ${profile.motherTongue}</p>
                    <p class="card-text">Religion: ${profile.religion}</p>
                    <p class="card-text">Ethnicity: ${profile.ethnicity}</p>
                    <p class="card-text">Bio: ${profile.bio}</p>
                    <img src="${profile.profilePicture}" alt="Profile Picture" class="img-thumbnail">
                    <p class="card-text">Habit: ${profile.habit}</p>
                    <p class="card-text">Gender: ${profile.gender}</p>
                    <p class="card-text">Weight: ${profile.weight}</p>
                    <p class="card-text">Height: ${profile.height}</p>
                    <p class="card-text">Managed By Relation: ${profile.managedByRelation}</p>
                </div>
            </div>`;
        document.getElementById('profileDetails').innerHTML = profileDetails;
    } else {
        alert('Profile not found');
    }
}

// Function to edit profile
function editProfile() {
    const profileId = localStorage.getItem('currentProfileId');
    loadContent('edit-profile.html');
    // window.location.href = `edit-profile.html?profileId=${profileId}`;
}

// Function to remove profile
function removeProfile() {
    const profileId = localStorage.getItem('currentProfileId');
    let profiles = JSON.parse(localStorage.getItem('profiles'));
    profiles = profiles.filter(p => p.profileId !== profileId);
    localStorage.setItem('profiles', JSON.stringify(profiles));
    alert('Profile removed successfully!');
    window.location.href = 'profiles.html';
}

// Function to view profile
function viewProfile(profileId) {
    localStorage.setItem('currentProfileId', profileId);
    loadContent('view-profile.html');
    // window.location.href = 'view-profile.html';
}

// Function to initialize view profile page
document.addEventListener('DOMContentLoaded', function() {
    if (window.location.href.includes('view-profile.html')) {
        const profileId = localStorage.getItem('currentProfileId');
        if (profileId) {
            loadProfileDetails(parseInt(profileId));
        }
    }
});


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
