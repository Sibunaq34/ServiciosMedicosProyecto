let sidebarExpanded = true;

function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const labels = document.querySelectorAll('.nav-label');
    const brandLabel = document.getElementById('brand-label');
    const toggleIcon = document.getElementById('toggle-icon');

    sidebarExpanded = !sidebarExpanded;

    if (sidebarExpanded) {
        sidebar.style.width = '230px';
        brandLabel.style.display = '';
        labels.forEach(function (l) { l.style.display = ''; });
        toggleIcon.className = 'bi bi-layout-sidebar-reverse sidebar-icon';
    } else {
        sidebar.style.width = '58px';
        brandLabel.style.display = 'none';
        labels.forEach(function (l) { l.style.display = 'none'; });
        toggleIcon.className = 'bi bi-layout-sidebar sidebar-icon';
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const links = document.querySelectorAll('.nav-item-link');
    links.forEach(function (link) {
        if (link.href && link.href === window.location.href) {
            link.classList.add('active-nav');
        }
    });
});
