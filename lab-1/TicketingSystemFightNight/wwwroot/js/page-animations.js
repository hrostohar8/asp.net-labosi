/* page-animations.js
   Global page transitions, scroll animations, reveal on scroll, scroll-to-top
*/
const PageAnimations = (() => {
  const throttle = (fn, wait) => {
    let last = 0;
    return (...args) => {
      const now = Date.now();
      if (now - last >= wait) { last = now; fn.apply(this, args); }
    };
  };

  function fadeInMain() {
    const main = document.getElementById('main-content');
    if (!main) return;
    main.style.opacity = '0';
    main.style.transform = 'translateY(6px)';
    main.style.transition = 'opacity 400ms ease, transform 400ms ease';
    requestAnimationFrame(() => {
      main.style.opacity = '1';
      main.style.transform = 'translateY(0)';
    });
  }

  function setupScrollReveal() {
    const elems = Array.from(document.querySelectorAll('.reveal-on-scroll'));
    if (!elems.length) return;
    const onScroll = throttle(() => {
      const vh = window.innerHeight;
      elems.forEach((el) => {
        if (el.getAttribute('data-revealed') === '1') return;
        const rect = el.getBoundingClientRect();
        if (rect.top < vh - 80) {
          el.setAttribute('data-revealed', '1');
          el.style.transition = 'opacity 420ms ease, transform 420ms ease';
          el.style.opacity = '1';
          el.style.transform = 'translateY(0)';
        }
      });
    }, 120);
    window.addEventListener('scroll', onScroll, { passive: true });
    // run once
    onScroll();
  }

  function setupScrollToTop() {
    const btn = document.createElement('button');
    btn.className = 'scroll-to-top';
    btn.setAttribute('aria-label', 'Scroll to top');
    btn.innerHTML = '↑';
    btn.style.opacity = '0';
    btn.style.pointerEvents = 'none';
    btn.addEventListener('click', () => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
    document.body.appendChild(btn);
    const onScroll = throttle(() => {
      if (window.scrollY > 300) {
        btn.style.opacity = '1'; btn.style.pointerEvents = 'auto';
      } else { btn.style.opacity = '0'; btn.style.pointerEvents = 'none'; }
    }, 100);
    window.addEventListener('scroll', onScroll, { passive: true });
  }

  function setupCardHoverEffects() {
    document.querySelectorAll('.entity-card').forEach((card) => {
      card.style.transition = 'transform 300ms ease, box-shadow 300ms ease';
      card.addEventListener('mouseenter', () => { card.style.transform = 'translateY(-6px)'; card.style.boxShadow = '0 18px 60px rgba(0,0,0,0.45)'; });
      card.addEventListener('mouseleave', () => { card.style.transform = ''; card.style.boxShadow = ''; });
    });
  }

  function setupRevealDefaults() {
    document.querySelectorAll('.reveal-on-scroll').forEach((el) => {
      el.style.opacity = '0';
      el.style.transform = 'translateY(14px)';
    });
  }

  function init() {
    try {
      fadeInMain();
      setupRevealDefaults();
      setupScrollReveal();
      setupScrollToTop();
      setupCardHoverEffects();
    } catch (e) { console.error('PageAnimations init failed', e); }
  }

  return { init };
})();

document.addEventListener('DOMContentLoaded', () => PageAnimations.init());
