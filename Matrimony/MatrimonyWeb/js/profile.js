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
