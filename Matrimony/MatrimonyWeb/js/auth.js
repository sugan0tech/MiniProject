
function login() {
    localStorage.setItem('isAuthenticated', 'true');
    console.log("Logged in successfully")
    window.location.href = 'index.html';
}

function register() {
    console.log("Registered check for your mail for activation")
}

function logout() {
    localStorage.setItem('isAuthenticated', 'false');
    window.location.href = 'login.html';
}
