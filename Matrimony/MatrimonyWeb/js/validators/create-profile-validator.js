document.getElementById('createProfileForm').addEventListener('submit', function(event) {
    event.preventDefault();
    let valid = true;

    // Date of Birth validation
    const dateOfBirth = new Date(document.getElementById('dateOfBirth').value);
    const age = new Date().getFullYear() - dateOfBirth.getFullYear();
    if (age < 21) {
        document.getElementById('dateOfBirth').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('dateOfBirth').classList.remove('is-invalid');
    }

    // Annual Income validation
    const annualIncome = document.getElementById('annualIncome').value;
    if (annualIncome <= 0) {
        document.getElementById('annualIncome').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('annualIncome').classList.remove('is-invalid');
    }

    // Weight validation
    const weight = document.getElementById('weight').value;
    if (weight <= 25) {
        document.getElementById('weight').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('weight').classList.remove('is-invalid');
    }

    // Height validation
    const height = document.getElementById('height').value;
    if (height <= 0 || height > 272) { // Considering 272 cm (8 feet 11.1 inches) as max height
        document.getElementById('height').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('height').classList.remove('is-invalid');
    }

    // Profile Picture validation
    const profilePicture = document.getElementById('profilePicture').files.length;
    if (profilePicture === 0) {
        document.getElementById('profilePicture').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('profilePicture').classList.remove('is-invalid');
    }

    // First Name and Last Name validation
    const namePattern = /^[A-Za-z]+$/;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    if (!namePattern.test(firstName)) {
        document.getElementById('firstName').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('firstName').classList.remove('is-invalid');
    }
    if (!namePattern.test(lastName)) {
        document.getElementById('lastName').classList.add('is-invalid');
        valid = false;
    } else {
        document.getElementById('lastName').classList.remove('is-invalid');
    }

    if (valid) {
        this.submit();
    }
});