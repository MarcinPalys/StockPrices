const API_BASE = "https://localhost:7235/api/StockPrices";
let chart = null;

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

function clearTable() {
    document.querySelector("#resultsTable tbody").innerHTML = "";
}
