const API_BASE = "https://localhost:7235/api/StockPrices";
const urlParams = new URLSearchParams(window.location.search);
const symbol = urlParams.get("symbol");

document.getElementById("heading").textContent = `Wykres dla: ${symbol}`;

fetch(`${API_BASE}/symbol/${symbol}`)
    .then(res => {
        if (!res.ok) throw new Error("Nie udało się pobrać danych.");
        return res.json();
    })
    .then(data => {
        drawChart(data);
    })
    .catch(err => {
        alert("Błąd: " + err.message);
    });

function drawChart(data) {
    data.sort((a, b) => new Date(a.date) - new Date(b.date));

    const ctx = document.getElementById("stockChart").getContext("2d");
    const labels = data.map(d => d.date);
    const values = data.map(d => d.close);

    new Chart(ctx, {
        type: "line",
        data: {
            labels: labels,
            datasets: [{
                label: "Cena zamknięcia (Close)",
                data: values,
                borderColor: "blue",
                fill: false,
                tension: 0.1
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: { title: { display: true, text: "Data" } },
                y: { title: { display: true, text: "Cena" } }
            }
        }
    });
}
