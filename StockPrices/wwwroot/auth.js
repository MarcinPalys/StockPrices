const API_BASE = "https://localhost:7235/api/account";

document.addEventListener("DOMContentLoaded", () => {
    const registerForm = document.getElementById("registerForm");
    const loginForm = document.getElementById("loginForm");

    if (registerForm) {
        registerForm.addEventListener("submit", async (e) => {
            e.preventDefault();
            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;

            const res = await fetch(`${API_BASE}/register`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password })
            });

            if (res.ok) { 
                alert("Rejestracja zakończona sukcesem. Możesz się zalogować.");
                window.location.href = "login.html";
            } else {
                alert("Błąd rejestracji.");
            }
        });
    }

    if (loginForm) {
        loginForm.addEventListener("submit", async (e) => {
            e.preventDefault();
            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;

            const res = await fetch(`${API_BASE}/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password })
            });

            if (res.ok) {
                const token = await res.text();
                localStorage.setItem("jwtToken", token);
                alert("Zalogowano pomyślnie!");
                window.location.href = "index.html"; // zmień jeśli inna strona startowa
            } else {
                alert("Nieprawidłowe dane logowania.");
            }
        });
    }
});
