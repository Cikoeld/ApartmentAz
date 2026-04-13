// ═══════════════════════════════════════════════════════════════════
// ApartmentAz — Shared JS  (interactions, theme, helpers)
// ═══════════════════════════════════════════════════════════════════

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

// ── Toggle Favorite ──────────────────────────────────────────────

async function toggleFavorite(listingId, btn) {
    const isFav = btn.dataset.fav === 'true';
    try {
        const res = await fetch(`/Favorite/Toggle?listingId=${listingId}&isFavorite=${isFav}`, { method: 'POST' });
        if (!res.ok) return;

        const nowFav = !isFav;
        btn.dataset.fav = nowFav.toString();

        const icon = btn.querySelector('i');
        if (icon) icon.className = nowFav ? 'bi bi-heart-fill' : 'bi bi-heart';

        const span = btn.querySelector('span');
        if (span) span.textContent = nowFav ? 'Remove from Favorites' : 'Save to Favorites';

        // Card overlay button (round)
        if (btn.classList.contains('card-fav-btn')) {
            btn.classList.toggle('active', nowFav);
        } else {
            // Detail page full-width button
            if (nowFav) btn.classList.replace('btn-outline-danger', 'btn-danger');
            else btn.classList.replace('btn-danger', 'btn-outline-danger');
        }

        // Pulse animation
        btn.style.transform = 'scale(1.2)';
        setTimeout(() => { btn.style.transform = ''; }, 200);
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

// ── On DOM ready ─────────────────────────────────────────────────

document.addEventListener('DOMContentLoaded', function () {
    // Sync theme icon
    const saved = localStorage.getItem('theme') || 'light';
    updateThemeIcon(saved);

    // Add stat-card top stripe color
    document.querySelectorAll('.admin-stat-card').forEach(card => {
        const color = card.style.getPropertyValue('--stripe');
        if (color && card.querySelector('::before') === null) {
            const before = card.querySelector('.card-body');
            if (before) card.style.borderTop = `4px solid ${color}`;
        }
    });
});
