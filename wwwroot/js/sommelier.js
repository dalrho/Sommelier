// Sommelier JavaScript Interop
window.sommelier = {
    saveTastingMenu: function (json) {
        try {
            localStorage.setItem('sommelier_tasting_menu', json);
            return true;
        } catch (e) {
            console.error('Failed to save tasting menu to localStorage', e);
            return false;
        }
    },

    loadTastingMenu: function () {
        try {
            return localStorage.getItem('sommelier_tasting_menu') || '';
        } catch (e) {
            console.error('Failed to load tasting menu from localStorage', e);
            return '';
        }
    },

    scrollToElement: function (elementId) {
        const el = document.getElementById(elementId);
        if (el) {
            el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    },

    scrollToTop: function () {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
};
