document.addEventListener('DOMContentLoaded', async () => {
    await loadProfileReports();

    document.getElementById('delete-profile-btn').addEventListener('click', async () => {
        const profileId = document.getElementById('profile-id-input').value;
        await deleteProfile(profileId);
    });
});

async function loadProfileReports() {
    try {
        const reports = await makeAuthRequest('Report');
        displayReports(reports);
    } catch (error) {
        console.error('Failed to load profile reports:', error);
    }
}

function displayReports(reports) {
    const reportList = document.getElementById('report-list');
    reportList.innerHTML = reports.map(report => `
        <div class="card mb-3">
            <div class="card-body">
                <h5 class="card-title">Report Details</h5>
                <p class="card-text">Profile ID: ${report.profileId}</p>
                <p class="card-text">Reported by User ID: ${report.reportedById}</p>
                <button class="btn btn-danger" onclick="deleteProfile(${report.profileId})">Delete Profile</button>
            </div>
        </div>
    `).join('');
}

async function deleteProfile(profileId) {
    try {
        await makeAuthRequest(`Profile/${profileId}`, 'DELETE');
        alert('Profile deleted successfully.');
    } catch (error) {
        console.error('Failed to delete profile:', error);
    }
}
