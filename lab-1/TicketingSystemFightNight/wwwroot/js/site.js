// Basic JavaScript for Ticketing System Fight Night

document.addEventListener('DOMContentLoaded', function() {
    // Add click tracking for navigation
    const navLinks = document.querySelectorAll('.nav-menu a');
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            // Add loading animation
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = '';
            }, 150);
        });
    });

    // Add table row highlighting with enhanced effects
    const tableRows = document.querySelectorAll('tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.background = 'linear-gradient(90deg, rgba(102, 126, 234, 0.1), rgba(118, 75, 162, 0.1))';
            this.style.transform = 'translateX(5px)';
            this.style.boxShadow = '0 5px 15px rgba(0,0,0,0.1)';
        });
        row.addEventListener('mouseleave', function() {
            this.style.background = '';
            this.style.transform = '';
            this.style.boxShadow = '';
        });
    });

    // Add smooth scrolling for anchor links
    const anchorLinks = document.querySelectorAll('a[href^="#"]');
    anchorLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Add fade-in animation for content
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    // Observe elements for animation
    const animateElements = document.querySelectorAll('.dashboard-item, .details-container, table, .detail-item');
    animateElements.forEach(el => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(30px)';
        el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(el);
    });

    // Add dynamic title updates
    const pageTitle = document.querySelector('h1, h2');
    if (pageTitle) {
        document.title = pageTitle.textContent + ' - Fight Night';
    }

    // Add keyboard navigation for tables
    const tables = document.querySelectorAll('table');
    tables.forEach(table => {
        const rows = table.querySelectorAll('tbody tr');
        let currentIndex = -1;

        table.addEventListener('keydown', function(e) {
            if (e.key === 'ArrowDown' && currentIndex < rows.length - 1) {
                currentIndex++;
                updateRowSelection();
            } else if (e.key === 'ArrowUp' && currentIndex > 0) {
                currentIndex--;
                updateRowSelection();
            } else if (e.key === 'Enter' && currentIndex >= 0) {
                const link = rows[currentIndex].querySelector('a');
                if (link) link.click();
            }
        });

        function updateRowSelection() {
            rows.forEach((row, index) => {
                if (index === currentIndex) {
                    row.style.background = 'rgba(102, 126, 234, 0.2)';
                    row.style.transform = 'scale(1.02)';
                } else {
                    row.style.background = '';
                    row.style.transform = '';
                }
            });
        }

        // Make table focusable
        table.setAttribute('tabindex', '0');
    });

    // Add loading states for links
    const allLinks = document.querySelectorAll('a');
    allLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            if (this.hostname === window.location.hostname) {
                // Add loading class for same-origin links
                this.classList.add('loading');
            }
        });
    });

    // Add responsive navigation toggle (for mobile)
    const navMenu = document.querySelector('.nav-menu');
    if (navMenu) {
        const toggleButton = document.createElement('button');
        toggleButton.innerHTML = '☰';
        toggleButton.style.display = 'none';
        toggleButton.style.background = 'none';
        toggleButton.style.border = 'none';
        toggleButton.style.color = '#ecf0f1';
        toggleButton.style.fontSize = '1.5rem';
        toggleButton.style.cursor = 'pointer';

        // Add toggle functionality for mobile
        if (window.innerWidth <= 768) {
            toggleButton.style.display = 'block';
            navMenu.style.display = 'none';
            navMenu.style.flexDirection = 'column';
            navMenu.style.position = 'absolute';
            navMenu.style.top = '100%';
            navMenu.style.left = '0';
            navMenu.style.right = '0';
            navMenu.style.background = 'rgba(44, 62, 80, 0.95)';
            navMenu.style.padding = '20px';

            toggleButton.addEventListener('click', function() {
                navMenu.style.display = navMenu.style.display === 'none' ? 'flex' : 'none';
            });
        }

        document.querySelector('.nav-container').appendChild(toggleButton);
    }

    // Add parallax effect for background
    window.addEventListener('scroll', function() {
        const scrolled = window.pageYOffset;
        const body = document.body;
        body.style.backgroundPositionY = -(scrolled * 0.5) + 'px';
    });

    // Add tooltip functionality
    const tooltipElements = document.querySelectorAll('[data-tooltip]');
    tooltipElements.forEach(el => {
        el.addEventListener('mouseenter', function(e) {
            const tooltip = document.createElement('div');
            tooltip.textContent = this.getAttribute('data-tooltip');
            tooltip.style.position = 'absolute';
            tooltip.style.background = 'rgba(0,0,0,0.8)';
            tooltip.style.color = 'white';
            tooltip.style.padding = '5px 10px';
            tooltip.style.borderRadius = '5px';
            tooltip.style.fontSize = '0.8rem';
            tooltip.style.pointerEvents = 'none';
            tooltip.style.zIndex = '1000';
            tooltip.style.top = e.pageY + 10 + 'px';
            tooltip.style.left = e.pageX + 10 + 'px';

            document.body.appendChild(tooltip);

            el.addEventListener('mouseleave', function() {
                tooltip.remove();
            });

            el.addEventListener('mousemove', function(e) {
                tooltip.style.top = e.pageY + 10 + 'px';
                tooltip.style.left = e.pageX + 10 + 'px';
            });
        });
    });
});