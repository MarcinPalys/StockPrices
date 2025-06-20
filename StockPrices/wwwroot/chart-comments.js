const COMMENTS_API = "https://localhost:7235/api/comments";
const token = localStorage.getItem("jwtToken");

const commentForm = document.getElementById("commentForm");
const commentText = document.getElementById("commentText");
const commentList = document.getElementById("commentList");

// Ukryj formularz, jeśli nie ma tokenu
if (!token) {
    commentForm.style.display = "none";
}

// Załaduj komentarze po załadowaniu strony
document.addEventListener("DOMContentLoaded", loadComments);

function loadComments() {
    if (!token) return;

    fetch(`${COMMENTS_API}/${symbol}`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
        .then(res => {
            if (!res.ok) throw new Error("Błąd ładowania komentarzy.");
            return res.json();
        })
        .then(data => {
            console.log("Komentarze z API:", data); // ← MUSI BYĆ TU!
            renderComments(data);
        })
        .catch(err => {
            console.error(err);
            commentList.innerHTML = `<li class="list-group-item text-danger">Błąd: ${err.message}</li>`;
        });
}

function renderComments(comments) {
    commentList.innerHTML = "";

    if (comments.length === 0) {
        commentList.innerHTML = `<li class="list-group-item">Brak komentarzy.</li>`;
        return;
    }

    comments.forEach(comment => {
        const li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between align-items-center";
        li.innerHTML = `
  <div class="w-100 d-flex justify-content-between align-items-center">
    <div>
      <span class="comment-text">${comment.text}</span>
      <small class="text-muted ms-2">(${new Date(comment.createdAt).toLocaleString()})</small>
    </div>
    <div class="btn-group btn-group-sm">
      <button class="btn btn-outline-secondary" onclick="editComment(${comment.id}, '${comment.text.replace(/'/g, "\\'")}')">Edytuj</button>
      <button class="btn btn-outline-danger" onclick="deleteComment(${comment.id})">Usuń</button>
    </div>
  </div>
`;

        commentList.appendChild(li);
    });
}

commentForm.addEventListener("submit", e => {
    e.preventDefault();
    const text = commentText.value.trim();
    if (!text) return;

    fetch(COMMENTS_API, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify({ symbol, text })
    })
        .then(res => {
            if (!res.ok) throw new Error("Nie udało się dodać komentarza.");
            return res.json();
        })
        .then(() => {
            commentText.value = "";
            loadComments();
        })
        .catch(err => alert(err.message));
});

function editComment(id, oldText) {
    const newText = prompt("Edytuj komentarz:", oldText);
    if (newText === null || newText.trim() === "") return;

    fetch(`${COMMENTS_API}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(newText)
    })
        .then(res => {
            if (!res.ok) throw new Error("Nie udało się zaktualizować komentarza.");
            return res.json();
        })
        .then(() => loadComments())
        .catch(err => alert(err.message));
}

function deleteComment(id) {
    if (!confirm("Na pewno chcesz usunąć komentarz?")) return;

    fetch(`${COMMENTS_API}/${id}`, {
        method: "DELETE",
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
        .then(res => {
            if (res.ok) {
                loadComments();
            } else {
                alert("Nie udało się usunąć komentarza.");
            }
        });
}
