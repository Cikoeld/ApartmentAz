// ── Shared helpers ───────────────────────────────────────────────

async function loadOptions(url, selectElement, placeholder) {
    selectElement.innerHTML = `<option value="">${placeholder}</option>`;
    if (!url) return;
    try {
        const data = await fetch(url).then(r => r.json());
        data.forEach(item => {
            const opt = document.createElement('option');
            opt.value = item.id;
            opt.textContent = item.name;
            selectElement.appendChild(opt);
        });
    } catch (e) {
        console.error('Failed to load options:', e);
    }
}

async function toggleFavorite(listingId, btn) {
    const isFav = btn.dataset.fav === 'true';
    try {
        const res = await fetch(`/Favorite/Toggle?listingId=${listingId}&isFavorite=${isFav}`, { method: 'POST' });
        if (!res.ok) return;

        const nowFav = !isFav;
        btn.dataset.fav = nowFav.toString();

        // Icon
        const icon = btn.querySelector('i');
        if (icon) icon.className = nowFav ? 'bi bi-heart-fill' : 'bi bi-heart';

        // Text label (only on detail page button)
        const span = btn.querySelector('span');
        if (span) span.textContent = nowFav ? 'Remove from Favorites' : 'Save to Favorites';

        // Button variant
        if (nowFav) {
            btn.classList.replace('btn-outline-danger', 'btn-danger');
        } else {
            btn.classList.replace('btn-danger', 'btn-outline-danger');
        }
    } catch (e) {
        console.error('Failed to toggle favorite:', e);
    }
}

// ── Light / Dark theme ───────────────────────────────────────────

function toggleTheme() {
    const html = document.documentElement;
    const current = html.getAttribute('data-bs-theme') || 'light';
    const next = current === 'dark' ? 'light' : 'dark';
    html.setAttribute('data-bs-theme', next);
    localStorage.setItem('theme', next);
    updateThemeIcon(next);
}

function updateThemeIcon(theme) {
    const icon = document.getElementById('themeIcon');
    if (!icon) return;
    icon.className = theme === 'dark' ? 'bi bi-sun-fill' : 'bi bi-moon-stars-fill';
}

// Sync the icon on every page load
document.addEventListener('DOMContentLoaded', function () {
    const saved = localStorage.getItem('theme') || 'light';
    updateThemeIcon(saved);
});
