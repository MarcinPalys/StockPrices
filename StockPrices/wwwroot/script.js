const API_BASE = "https://localhost:7235/api/StockPrices";
let chart = null;
let currentPage = 1;
const pageSize = 20;
const token = localStorage.getItem("jwtToken");

async function fetchAndDisplay(url) {
    try {
        const res = await fetch(url);
        if (!res.ok) throw new Error("Brak danych.");
        const data = await res.json();
        populateTable(Array.isArray(data) ? data : [data]);
    } catch (error) {
        alert("Błąd: " + error.message);
        clearTable();
    }
}
function renderAuthButtons() {
    const container = document.getElementById("authButtons");

    if (token) {
        // zalogowany
        container.innerHTML = `
      <button class="btn btn-outline-danger" onclick="logout()">Wyloguj się</button>
    `;
    } else {
        // niezalogowany
        container.innerHTML = `
      <a href="login.html" class="btn btn-outline-primary me-2">Zaloguj się</a>
      <a href="register.html" class="btn btn-outline-secondary">Zarejestruj się</a>
    `;
    }
}

function logout() {
    localStorage.removeItem("jwtToken");
    window.location.href = "index.html";
}

function fetchAll() {
    fetchAndDisplay(`${API_BASE}`);
}

function fetchBySymbol() {
    const symbol = document.getElementById("symbolInput").value.trim();
    if (!symbol) return alert("Podaj symbol!");
    fetchAndDisplay(`${API_BASE}/symbol/${symbol}`);
}

function fetchByDate() {
    const date = document.getElementById("dateInput").value;
    if (!date) return alert("Podaj datę!");
    fetchAndDisplay(`${API_BASE}/date/${date}`);
}

function fetchBySymbolAndDate() {
    const symbol = document.getElementById("symbolInput").value.trim();
    const date = document.getElementById("dateInput").value;
    if (!symbol || !date) return alert("Podaj symbol i datę!");
    fetchAndDisplay(`${API_BASE}/symbol/${symbol}/date/${date}`);
}

function fetchByRange() {
    const symbol = document.getElementById("symbolInput").value.trim();
    const from = document.getElementById("fromDate").value;
    const to = document.getElementById("toDate").value;
    if (!symbol || !from || !to) return alert("Uzupełnij symbol i zakres dat!");
    fetchAndDisplay(`${API_BASE}/symbol/${symbol}/range?from=${from}&to=${to}`);
}

function populateTable(data) {
    const tbody = document.querySelector("#resultsTable tbody");
    const tr = document.createElement("tr");
    tr.classList.add("clickable-row");

    tbody.innerHTML = "";

    data.forEach(row => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
      <td>${row.symbol}</td>
      <td>${row.date}</td>
      <td>${row.open}</td>
      <td>${row.high}</td>
      <td>${row.low}</td>
      <td>${row.close}</td>
      <td>${row.volume}</td>
    `;

        tr.addEventListener("click", () => {
            window.location.href = `chart.html?symbol=${row.symbol}`;
        });

        tbody.appendChild(tr);
    });
}
function fetchPage(page) {
    currentPage = page;

    fetch(`${API_BASE}/page?page=${page}&pageSize=${pageSize}`)
        .then(res => {
            if (!res.ok) throw new Error("Nie udało się pobrać danych.");
            return res.json();
        })
        .then(data => {
            populateTable(data);
            renderPagination(); // ← dodaj nową paginację po pobraniu
        })
        .catch(err => {
            alert(err.message);
            clearTable();
        });
}
document.addEventListener("DOMContentLoaded", () => {
    renderAuthButtons();
    fetchPage(1);
});
function renderPagination() {
    const pagination = document.getElementById("pagination");
    pagination.innerHTML = "";
    console.log("Rysuję paginację")
    const prevBtn = document.createElement("li");
    prevBtn.className = `page-item ${currentPage === 1 ? "disabled" : ""}`;
    prevBtn.innerHTML = `<a class="page-link" href="#">Poprzednia</a>`;
    prevBtn.onclick = () => {
        if (currentPage > 1) fetchPage(currentPage - 1);
    };

    const nextBtn = document.createElement("li");
    nextBtn.className = "page-item";
    nextBtn.innerHTML = `<a class="page-link" href="#">Następna</a>`;
    nextBtn.onclick = () => fetchPage(currentPage + 1);

    pagination.appendChild(prevBtn);
    pagination.appendChild(nextBtn);
}


function clearTable() {
    document.querySelector("#resultsTable tbody").innerHTML = "";
}
