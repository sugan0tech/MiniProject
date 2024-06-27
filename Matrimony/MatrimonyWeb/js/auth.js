const baseUrl = 'http://localhost:5094/api/';
async function postAuthRequest(endpoint, data, headers) {
    const url = `${baseUrl}Auth/` + endpoint;
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

function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

async function adminValidator(){
    if (localStorage.getItem("isAuthenticated")){
        var role = parseJwt(localStorage.getItem("accessToken"))["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        if (role === "Admin")
            window.location.href = 'admin-dashboard.html';
    }
}

async function userValidator(){
    if (localStorage.getItem("isAuthenticated")){
        var role = parseJwt(localStorage.getItem("accessToken"))["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        if (role === "User")
            window.location.href = 'index.html';
    }
}

async function login(email, password, staySigned) {
    const loginEndpoint = 'login';
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'text/plain',
    };
    const data = {
        email: email,
        password: password,
        staySigned: staySigned
    };

    try {
        const response = await postAuthRequest(loginEndpoint, data, headers);
        if (response.status && response.status !== 200){
            showAlert(`Login failed :${response.message}`, 'danger')
            return;
        }
        setAuthTokens(response.accessToken, response.refreshToken);

        var role = parseJwt(localStorage.getItem("accessToken"))["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        console.log("Logged in successfully");
        showAlert("Login success", 'success')
        if (role === "Admin")
            window.location.href = 'admin-dashboard.html';
        else
            window.location.href = 'index.html';
    } catch (error) {
        console.error("Login failed:", error);
        alert("Login failed. Please check your credentials and try again.");
    }
}

async function handleLogin(event) {
    event.preventDefault(); // Prevent the form from submitting the default way

    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('pwd');
    const staySignedInput = document.getElementById('staySignedIn');

    const email = emailInput.value;
    const password = passwordInput.value;
    const staySigned = staySignedInput.checked;

    await login(email, password, staySigned);
}

document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('login-form');
    loginForm.addEventListener('submit', handleLogin);
});

// Register function modified to redirect to OTP entry
async function register(event) {
    event.preventDefault();
    const registerEndpoint = 'register';
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
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

        if (!response.success) {
            showAlert("Registration successful! Redirecting to OTP entry...", "success");
            setTimeout(() => {
                window.location.href = `otp-entry.html?userId=${response.userId}`;
            }, 2000);
        } else {
            showAlert("Registration failed. Please try again.", "danger");
        }
    } catch (error) {
        console.error("Registration failed:", error);
        showAlert("Registration failed. Please try again.", "danger");
    }
}

// OTP verification function
async function verifyOtp(event) {
    event.preventDefault();
    const userId = new URLSearchParams(window.location.search).get('userId');
    const otp = document.getElementById('otp').value;
    const otpEndpoint = `verify-otp/${userId}`;
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
    };

    try {
        const response = await postAuthRequest(otpEndpoint, otp, headers);

        if (response.success) {
            showAlert("OTP verified successfully! Redirecting to login...", "success");
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 2000);
        } else {
            showAlert("Invalid OTP. Contact Admin for activation", "danger");
        }
    } catch (error) {
        console.error("OTP verification failed:", error);
        showAlert("OTP verification failed. Please try again.", "danger");
    }
}

// Event listeners
document.addEventListener('DOMContentLoaded', () => {
    const registerForm = document.getElementById('register-form');
    if (registerForm) {
        registerForm.addEventListener('submit', register);
    }

    const otpForm = document.getElementById('otp-form');
    if (otpForm) {
        otpForm.addEventListener('submit', verifyOtp);
    }
})


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
        localStorage.clear();

        console.log("Logged out successfully");
        window.location.href = 'login.html';
    } catch (error) {
        console.error("Logout failed:", error);
        alert("Logout failed. Please try again.");
    }
}
async function makeAuthRequest(endpoint, method = 'GET', data = null) {
    let url = baseUrl + endpoint;
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'text/plain',
        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
    };

    let response;
    try {
        response = await fetch(url, {
            method: method,
            headers: headers,
            body: data ? JSON.stringify(data) : null
        });

        if (response.status === 401) {
            const refreshSuccess = await refreshAccessToken();
            if (!refreshSuccess) {
                // await logout()
                showAlert("you have logged out", 'danger')
                return null;
            }

            // Retry the original request with a new token
            headers['Authorization'] = `Bearer ${localStorage.getItem('accessToken')}`;
            response = await fetch(url, {
                method: method,
                headers: headers,
                body: data ? JSON.stringify(data) : null
            });
        }

        if (!response.ok) {
            console.log(response.json().then(value => showAlert(value.message, 'danger')))
            console.log(response.status)
            return response;
        }
        const contentType = response.headers.get('Content-Type');
        return contentType && contentType.includes('application/json') ? await response.json() : null;
    } catch (error) {
        console.error('Request failed:', error);
        if (response && response.status === 401) await logout(); // Log out if refreshing token failed
        throw error;
    }
}

// Function to refresh access token
async function refreshAccessToken() {
    const refreshToken = localStorage.getItem('refreshToken');
    if (!refreshToken) return false;

    try {
        const response = await fetch('http://localhost:5094/api/Auth/access-token', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'text/plain',
                'Authorization': `Bearer ${refreshToken}`
            }
        });

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        const data = await response.json();
        localStorage.setItem('accessToken', data.accessToken);
        return true;
    } catch (error) {
        console.error('Token refresh failed:', error);
        await logout();
        return false;
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