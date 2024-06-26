// function profileSelectionChanged() {
//     const selectedProfileId = document.getElementById('userProfile').value;
//     console.log("Selected profile ID:", selectedProfileId);
// }

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

//
// function updateAgeRangeDisplay() {
//     const ageRangeFilter = document.getElementById('ageRangeFilter').value;
//     document.getElementById('ageRangeDisplay').textContent = `${ageRangeFilter}`;
// }
//
// function updateHeightRangeDisplay() {
//     const heightRangeFilter = document.getElementById('heightRangeFilter').value;
//     document.getElementById('heightRangeDisplay').textContent = `${heightRangeFilter}`;
// }

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
        managedByRelation: document.getElementById('managedByRelation').value,
        managedById: parseInt(document.getElementById('managedById').value),
        userId: parseInt(document.getElementById('userId').value)
    };

    try {
        await makeAuthRequest('Profile', 'POST', profileData);
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

function loadProfile() {
    const profileId = localStorage.getItem("currentProfile");
    const profileData = JSON.parse(localStorage.getItem(`profile${profileId}`));
    console.log(profileId)
    console.log(profileData)
    let content = `
          <div class="card-body">
                <h5 class="card-title">Profile Details</h5>
                <div class="row">
                    <div class="col-md-6 mb-3">
                        <p id="profileId" class="card-text"><strong>Profile ID:</strong> ${profileData.profileId}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="dateOfBirth" class="card-text"><strong>Date of Birth:</strong> ${profileData.dateOfBirth}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="age" class="card-text"><strong>Age:</strong> ${profileData.age}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="education" class="card-text"><strong>Education:</strong> ${profileData.education}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="annualIncome" class="card-text"><strong>Annual Income:</strong> ${profileData.annualIncome}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="occupation" class="card-text"><strong>Occupation:</strong> ${profileData.occupation}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="maritalStatus" class="card-text"><strong>Marital Status:</strong> ${profileData.maritalStatus}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="motherTongue" class="card-text"><strong>Mother Tongue:</strong> ${profileData.motherTongue}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="religion" class="card-text"><strong>Religion:</strong> ${profileData.religion}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="ethnicity" class="card-text"><strong>Ethnicity:</strong> ${profileData.ethnicity}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="bio" class="card-text"><strong>Bio:</strong> ${profileData.bio}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="habit" class="card-text"><strong>Habit:</strong> ${profileData.habit}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="gender" class="card-text"><strong>Gender:</strong> ${profileData.gender}</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="weight" class="card-text"><strong>Weight:</strong> ${profileData.weight} kg</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="height" class="card-text"><strong>Height:</strong> ${profileData.height} cm</p>
                    </div>
                    <div class="col-md-6 mb-3">
                        <p id="managedByRelation" class="card-text"><strong>Managed By Relation:</strong> ${profileData.managedByRelation}</p>
                    </div>
                    <!--
                    <div class="col-12">
                         <img id="profilePicture" src="${profileData.profilePicture}" alt="Profile Picture" class="img-thumbnail">
                    </div>
                    -->
                </div>
            </div>`;
    document.getElementById("profileDetails").innerHTML = content;
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


function loadForEditProfile() {
    const profileId = localStorage.getItem("currentProfile");
    const profileData = JSON.parse(localStorage.getItem(`profile${profileId}`));

    console.log(profileId);
    console.log(profileData);

    document.getElementById('profileId').value = profileData.profileId;
    document.getElementById('managedById').value = profileData.managedById;
    document.getElementById('userId').value = profileData.userId;
    document.getElementById('dateOfBirth').value = profileData.dateOfBirth.split('T')[0];
    document.getElementById('age').value = profileData.age;
    document.getElementById('education').value = profileData.education;
    document.getElementById('annualIncome').value = profileData.annualIncome;
    document.getElementById('occupation').value = profileData.occupation;
    document.getElementById('maritalStatus').value = profileData.maritalStatus;
    document.getElementById('motherTongue').value = profileData.motherTongue;
    document.getElementById('religion').value = profileData.religion;
    document.getElementById('ethnicity').value = profileData.ethnicity;
    document.getElementById('bio').value = profileData.bio;
    document.getElementById('habit').value = profileData.habit;
    document.getElementById('gender').value = profileData.gender;
    document.getElementById('weight').value = profileData.weight;
    document.getElementById('height').value = profileData.height;
    document.getElementById('managedByRelation').value = profileData.managedByRelation;
    document.getElementById('profilePicture').value = profileData.profilePicture;
}

function saveChanges() {
    const profileId = localStorage.getItem("currentProfileId");
    const updatedProfile = {
        profileId: parseInt(document.getElementById('profileId').value),
        managedById: parseInt(document.getElementById('managedById').value),
        userId: parseInt(document.getElementById('userId').value),
        dateOfBirth: document.getElementById('dateOfBirth').value,
        age: parseInt(document.getElementById('age').value),
        education: document.getElementById('education').value,
        annualIncome: parseInt(document.getElementById('annualIncome').value),
        occupation: document.getElementById('occupation').value,
        maritalStatus: document.getElementById('maritalStatus').value,
        motherTongue: document.getElementById('motherTongue').value,
        religion: document.getElementById('religion').value,
        ethnicity: document.getElementById('ethnicity').value,
        bio: document.getElementById('bio').value,
        habit: document.getElementById('habit').value,
        gender: document.getElementById('gender').value,
        weight: parseFloat(document.getElementById('weight').value),
        height: parseFloat(document.getElementById('height').value),
        managedByRelation: document.getElementById('managedByRelation').value,
        // profilePicture: document.getElementById('profilePicture').value
    };

    let response = makeAuthRequest("profile", "PUT", updatedProfile);
}

function loadSelectProfiles() {
    const keys = Object.keys(localStorage)
    let range = keys.length;
    let profiles = []
    while (range--) {
        if (keys[range].includes("profile") && !keys[range].includes("currentProfile"))
            profiles.push(JSON.parse(localStorage.getItem(keys[range])))
    }

    const profileSelect = document.getElementById('userProfile');
    profiles.forEach(profile => {
        console.log(profile)
        const option = document.createElement('option');
        option.value = profile.profileId;
        option.selected = findAndSetDefaults(profile);
        option.textContent = `id: ${profile.profileId} Name: ${profile.user.firstName} ${profile.user.lastName} `;
        profileSelect.appendChild(option);
    });
}

function findAndSetDefaults(profile) {
    if (localStorage.getItem("currentProfile") == profile.profileId) {
        fetchPreferences(profile.preferenceId);
        console.log("hereee")
        return true;
    }
    return false;
}

function profileSelectionChanged() {
    const selectedProfileId = document.getElementById('userProfile').value;
    localStorage.setItem('currentProfile', selectedProfileId);
    var profile = JSON.parse(localStorage.getItem('profile' + selectedProfileId))
    fetchPreferences(profile.preferenceId);
}

function fetchPreferences(profileId) {
    makeAuthRequest(`Preference/${profileId}`)
        .then(response => response)
        .then(data => updateFilters(data))
        .catch(error => console.error('Error fetching preferences:', error));
}

function updateFilters(preferences) {
    document.getElementById('genderFilter').value = preferences.gender || "";
    document.getElementById('motherTongueFilter').value = preferences.motherTongue || "";
    document.getElementById('religionFilter').value = preferences.religion || "";
    document.getElementById('educationFilter').value = preferences.education || "";
    document.getElementById('occupationFilter').value = preferences.occupation || "";
    document.getElementById('minAge').value = preferences.minAge || 18;
    document.getElementById('maxAge').value = preferences.maxAge || 60;
    document.getElementById('minHeight').value = preferences.minHeight || 140;
    document.getElementById('maxHeight').value = preferences.maxHeight || 210;
}

function applyFilter() {
    const profileId = JSON.parse(localStorage.getItem('currentProfile'));
    var profile = JSON.parse(localStorage.getItem('profile' + profileId))
    const preferences = {
        preferenceId: profile.preferenceId,
        gender: document.getElementById('genderFilter').value,
        motherTongue: document.getElementById('motherTongueFilter').value,
        religion: document.getElementById('religionFilter').value,
        education: document.getElementById('educationFilter').value,
        occupation: document.getElementById('occupationFilter').value,
        minHeight: parseInt(document.getElementById('minHeight').value),
        maxHeight: parseInt(document.getElementById('maxHeight').value),
        minAge: parseInt(document.getElementById('minAge').value),
        maxAge: parseInt(document.getElementById('maxAge').value),
        preferenceForId: profileId
    };

    makeAuthRequest("Preference", 'PUT', preferences)
        .then(data => {
            console.log('Preferences saved successfully:', data);
            alert('Preferences have been applied and saved.');
        })
        .catch(error => console.error('Error saving preferences:', error));

    fetchFilteredProfiles(profileId).then();
}

async function fetchFilteredProfiles(profileId) {
    const profiles = await makeAuthRequest("Profile/" + profileId + "/matches")
    console.log("profile previews")
    console.log(profiles)
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
    displayProfiles(profiles);
}

function displayProfiles(profiles) {
    const profilesList = document.getElementById('profilesList');
    // profilesList.innerHTML = ''; // Clear existing profiles

    profiles.forEach(profile => {
        const profileCard = `
            <div class="card mb-3" data-profile-id="${profile.profileId}">
                <div class="card-body">
                    <h5 class="card-title">Profile ${profile.profileId}: ${profile.profilePicture ? `<img src="${profile.profilePicture}" alt="Profile Picture" />` : 'No Picture'}</h5>
                    <p class="card-text">Education: ${profile.education}, Occupation: ${profile.occupation}</p>
                    <p class="card-text">Religion: ${profile.religion}, Ethnicity: ${profile.ethnicity}</p>
                    <button class="btn btn-info" onclick="viewProfile(${profile.profileId})">View Profile</button>
                    <button class="btn btn-danger" onclick="reportProfile(${profile.profileId})">Report Profile</button>
                    <button class="btn btn-primary" onclick="sendMatchRequest(${profile.profileId})">Chat</button>
                </div>
            </div>
        `;
        profilesList.innerHTML += profileCard;
    });


    // Apply pagination
    setupPagination(profiles.length);
}

function setupPagination(totalProfiles) {
    const profilesPerPage = 5;
    const totalPages = Math.ceil(totalProfiles / profilesPerPage);
    const paginationContainer = document.getElementById('pagination');

    paginationContainer.innerHTML = ''; // Clear existing pagination

    for (let i = 1; i <= totalPages; i++) {
        const pageLink = document.createElement('a');
        pageLink.href = '#';
        pageLink.innerText = i;
        pageLink.onclick = (e) => {
            e.preventDefault();
            displayPage(i);
        };
        paginationContainer.appendChild(pageLink);
    }

    displayPage(1); // Display the first page initially
}

function displayPage(pageNumber) {
    const profilesPerPage = 5;
    const profiles = Array.from(document.querySelectorAll('#profilesList .card'));
    const start = (pageNumber - 1) * profilesPerPage;
    const end = start + profilesPerPage;

    profiles.forEach((profile, index) => {
        if (index >= start && index < end) {
            profile.style.display = 'block';
        } else {
            profile.style.display = 'none';
        }
    });
}

function searchProfiles() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const profiles = document.querySelectorAll('#profilesList .card');

    profiles.forEach(profile => {
        const profileName = profile.querySelector('.card-title').innerText.toLowerCase();
        if (profileName.includes(searchInput)) {
            profile.style.display = 'block';
        } else {
            profile.style.display = 'none';
        }
    });
}

function sortProfiles() {
    const sortValue = document.getElementById('sortSelect').value;
    const profiles = Array.from(document.querySelectorAll('#profilesList .card'));

    profiles.sort((a, b) => {
        const aName = a.querySelector('.card-title').innerText;
        const bName = b.querySelector('.card-title').innerText;

        if (sortValue === 'nameAsc') {
            return aName.localeCompare(bName);
        } else if (sortValue === 'nameDesc') {
            return bName.localeCompare(aName);
        }
    });

    const profilesList = document.getElementById('profilesList');
    profilesList.innerHTML = ''; // Clear existing profiles
    profiles.forEach(profile => profilesList.appendChild(profile));
}
