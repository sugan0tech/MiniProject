async function validateAndSave() {
    let form = document.getElementById('profileForm');
    let isValid = true;

    // Validate Date of Birth
    let dateOfBirth = document.getElementById('dateOfBirth');
    let dob = new Date(dateOfBirth.value);
    let today = new Date();
    let age = today.getFullYear() - dob.getFullYear();
    let m = today.getMonth() - dob.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
        age--;
    }
    if (age < 21) {
        dateOfBirth.classList.add('is-invalid');
        isValid = false;
    } else {
        dateOfBirth.classList.remove('is-invalid');
    }

    const profilePicture = document.getElementById('profilePicture').files.length;
    if (profilePicture === 0) {
        document.getElementById('profilePicture').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('profilePicture').classList.remove('is-invalid');
    }

    // Validate Annual Income
    let annualIncome = document.getElementById('annualIncome');
    if (annualIncome.value <= 0) {
        annualIncome.classList.add('is-invalid');
        isValid = false;
    } else {
        annualIncome.classList.remove('is-invalid');
    }

    // Validate Weight
    let weight = document.getElementById('weight');
    if (weight.value <= 25) {
        weight.classList.add('is-invalid');
        isValid = false;
    } else {
        weight.classList.remove('is-invalid');
    }

    // Validate Height
    let height = document.getElementById('height');
    if (height.value <= 50) {
        height.classList.add('is-invalid');
        isValid = false;
    } else {
        height.classList.remove('is-invalid');
    }

    if (isValid) {
        await saveChanges();
    }
}