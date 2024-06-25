import { postRequest, putRequest, delRequest } from './httpUtils.js';
import { showAlert } from './common.js';
function profileSelectionChanged() {
    const selectedProfileId = document.getElementById('userProfile').value;
    console.log("Selected profile ID:", selectedProfileId);
}

function searchProfiles() {
    const query = document.getElementById('searchInput').value.toLowerCase();
    const profiles = document.querySelectorAll('#profilesList .card');
    profiles.forEach(profile => {
        const name = profile.getAttribute('data-name').toLowerCase();
        profile.style.display = name.includes(query) ? '' : 'none';
    });
}

function sortProfiles() {
    const sortBy = document.getElementById('sortSelect').value;
    const profiles = Array.from(document.querySelectorAll('#profilesList .card'));

    profiles.sort((a, b) => {
        const nameA = a.getAttribute('data-name').toLowerCase();
        const nameB = b.getAttribute('data-name').toLowerCase();
        if (sortBy === 'nameAsc') {
            return nameA < nameB ? -1 : (nameA > nameB ? 1 : 0);
        } else {
            return nameA > nameB ? -1 : (nameA < nameB ? 1 : 0);
        }
    });

    const profilesList = document.getElementById('profilesList');
    profilesList.innerHTML = '';
    profiles.forEach(profile => profilesList.appendChild(profile));
}

function filterProfiles() {
    const genderFilter = document.getElementById('genderFilter').value;
    const motherTongueFilter = document.getElementById('motherTongueFilter').value;
    const religionFilter = document.getElementById('religionFilter').value;
    const educationFilter = document.getElementById('educationFilter').value;
    const occupationFilter = document.getElementById('occupationFilter').value;
    const ageRangeFilter = document.getElementById('ageRangeFilter').value;
    const heightRangeFilter = document.getElementById('heightRangeFilter').value;

    const profiles = document.querySelectorAll('#profilesList .card');
    profiles.forEach(profile => {
        const gender = profile.getAttribute('data-gender');
        const motherTongue = profile.getAttribute('data-mother-tongue');
        const religion = profile.getAttribute('data-religion');
        const education = profile.getAttribute('data-education');
        const occupation = profile.getAttribute('data-occupation');
        const age = parseInt(profile.getAttribute('data-age'));
        const height = parseInt(profile.getAttribute('data-height'));

        let display = true;
        if (genderFilter && gender !== genderFilter) display = false;
        if (motherTongueFilter && motherTongue !== motherTongueFilter) display = false;
        if (religionFilter && religion !== religionFilter) display = false;
        if (educationFilter && education !== educationFilter) display = false;
        if (occupationFilter && occupation !== occupationFilter) display = false;
        if (ageRangeFilter && (age < parseInt(ageRangeFilter.split(',')[0]) || age > parseInt(ageRangeFilter.split(',')[1]))) display = false;
        if (heightRangeFilter && (height < parseInt(heightRangeFilter.split(',')[0]) || height > parseInt(heightRangeFilter.split(',')[1]))) display = false;

        profile.style.display = display ? '' : 'none';
    });
}

function updateAgeRangeDisplay() {
    const ageRangeFilter = document.getElementById('ageRangeFilter').value;
    document.getElementById('ageRangeDisplay').textContent = `${ageRangeFilter}`;
}

function updateHeightRangeDisplay() {
    const heightRangeFilter = document.getElementById('heightRangeFilter').value;
    document.getElementById('heightRangeDisplay').textContent = `${heightRangeFilter}`;
}

function viewProfile(profileId) {
    console.log("View profile with ID:", profileId);
}

function reportProfile(profileId) {
    console.log("Remove profile with ID:", profileId);
}

function sendMatchRequest(profileId) {
    console.log("Send match request to profile ID:", profileId);
}

const headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
};

// Function to handle profile creation
async function createProfile(event) {
    event.preventDefault();

    const profileData = {
        dateOfBirth: document.getElementById('dateOfBirth').value,
        education: document.getElementById('education').value,
        annualIncome: parseInt(document.getElementById('annualIncome').value),
        occupation: document.getElementById('occupation').value,
        maritalStatus: document.getElementById('maritalStatus').value,
        motherTongue: document.getElementById('motherTongue').value,
        religion: document.getElementById('religion').value,
        ethnicity: document.getElementById('ethnicity').value,
        bio: document.getElementById('bio').value,
        // profilePicture: await getBase64(document.getElementById('profilePicture').files[0]),
        habit: document.getElementById('habit').value,
        gender: document.getElementById('gender').value,
        weight: parseInt(document.getElementById('weight').value),
        height: parseInt(document.getElementById('height').value),
        managedByRelation: document.getElementById('managedByRelation').value
    };

    try {
        await postRequest('Profile', profileData, headers);
        showAlert("Profile created successfully.", 'success');
    } catch (error) {
        showAlert("Failed to create profile. Please try again.", 'danger');
        if (error.message === '401') await handleUnauthorizedError();
    }
}

// Convert file to base64
async function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

// Function to update a profile
async function updateProfile(profileId, profileData) {
    try {
        await putRequest(`Profile/${profileId}`, profileData, headers);
        showAlert("Profile updated successfully.", 'success');
    } catch (error) {
        showAlert("Failed to update profile. Please try again.", 'danger');
        if (error.message === '401') await handleUnauthorizedError();
    }
}

// Function to delete a profile by ID
async function deleteProfile(profileId) {
    try {
        await delRequest(`Profile/${profileId}`, {}, headers);
        showAlert("Profile deleted successfully.", 'success');
    } catch (error) {
        showAlert("Failed to delete profile. Please try again.", 'danger');
        if (error.message === '401') await handleUnauthorizedError();
    }
}

// Handle unauthorized error
async function handleUnauthorizedError() {
    const refreshSuccess = await refreshAccessToken();
    if (!refreshSuccess) {
        logout();
    } else {
        initialize(); // Reinitialize the app with new token
    }
}

// Event listener for form submission
document.getElementById('createProfileForm').addEventListener('submit', createProfile);
