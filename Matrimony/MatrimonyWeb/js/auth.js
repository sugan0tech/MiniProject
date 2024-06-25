async function postAuthRequest(endpoint, data, headers) {
    const url = 'http://localhost:5094/api/Auth/' + endpoint;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(data)
        });

        const contentType = response.headers.get('Content-Type');
        if (contentType && contentType.includes('application/json')) {
            return await response.json();
        } else {
            return response;
        }
    } catch (error) {
        console.error('Error during fetch:', error);
        throw error; 
    }
}

async function login(email, password) {
    const loginEndpoint = 'login';
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'text/plain',
    };
    const data = {
        email: email,
        password: password
    };

    try {
        const response = await postAuthRequest(loginEndpoint, data, headers);
        if (response.status && response.status !== 200){
            showAlert(`Login failed :${response.message}`, 'danger')
            return;
        }
        setAuthTokens(response.accessToken, response.refreshToken);

        console.log("Logged in successfully");
        showAlert("Login success", 'success')
        window.location.href = 'index.html';
    } catch (error) {
        console.error("Login failed:", error);
        alert("Login failed. Please check your credentials and try again.");
    }
}

function handleLogin(event) {
    event.preventDefault(); // Prevent the form from submitting the default way

    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('pwd');

    const email = emailInput.value;
    const password = passwordInput.value;

    login(email, password);
}

document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('login-form');
    loginForm.addEventListener('submit', handleLogin);
});

async function register(event) {
    event.preventDefault();
    const registerEndpoint = 'register';
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'text/plain',
    };
    const data = {
        email: document.getElementById('email').value,
        password: document.getElementById('pwd').value,
        firstName: document.getElementById('fname').value,
        lastName: document.getElementById('lname').value,
        phoneNumber: document.getElementById('phone').value
    };

    try {
        const response = await postAuthRequest(registerEndpoint, data, headers);

        if (response.success){
            showAlert("Oruvaliya jeichutom maara :heart:", "success")
        }
        else
        {
            showAlert("Adutha jenmathula papom murugesa", "danger")
        }


        console.log("Registered in successfully");
    } catch (error) {
        console.error("Register failed:", error);
        alert("Login failed. Please check your credentials and try again.");
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const registerForm = document.getElementById('register-form');
    registerForm.addEventListener('submit', register);
});


async function logout() {
    const logoutEndpoint = 'logout';
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');

    if (!accessToken || !refreshToken) {
        console.error("No valid session found.");
        alert("No active session to logout.");
        return;
    }

    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'text/plain',
        'Authorization': `Bearer ${refreshToken}`
    };

    try {
        await postAuthRequest(logoutEndpoint,{}, headers);

        clearAuthTokens();

        console.log("Logged out successfully");
        window.location.href = 'login.html';
    } catch (error) {
        console.error("Logout failed:", error);
        alert("Logout failed. Please try again.");
    }
}

function clearAuthTokens() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('isAuthenticated');
}

function setAuthTokens(accessToken, refreshToken) {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    localStorage.setItem('isAuthenticated', 'true');
}

function showAlert(message, type) {
    const alertContainer = document.getElementById('alert-container');
    const alertHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;
    alertContainer.innerHTML = alertHTML;
}