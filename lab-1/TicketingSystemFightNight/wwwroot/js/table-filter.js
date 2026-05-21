/* table-filter.js
   Real-time filtering for entity grids (cards or table rows).
   Uses vanilla JS, debounce, highlights matches, shows count, animates rows.
*/
const TableFilter = (() => {
  const debounce = (fn, wait) => {
    let t;
    return (...args) => {
      clearTimeout(t);
      t = setTimeout(() => fn.apply(this, args), wait);
    };
  };

  const escapeRegExp = (s) => s.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

  function highlightText(node, query) {
    if (!query) return;
    const re = new RegExp(`(${escapeRegExp(query)})`, 'ig');
    walk(node, (textNode) => {
      const parent = textNode.parentNode;
      if (!parent) return;
      const frag = document.createDocumentFragment();
      const parts = textNode.textContent.split(re);
      parts.forEach((p) => {
        if (!p) return;
        if (re.test(p)) {
          const mark = document.createElement('mark');
          mark.textContent = p;
          frag.appendChild(mark);
        } else {
          frag.appendChild(document.createTextNode(p));
        }
      });
      parent.replaceChild(frag, textNode);
    });
  }

  function walk(node, fn) {
    if (!node) return;
    if (node.nodeType === Node.TEXT_NODE) {
      fn(node);
    } else if (node.nodeType === Node.ELEMENT_NODE && node.tagName !== 'SCRIPT' && node.tagName !== 'STYLE' && node.tagName !== 'MARK') {
      for (let i = 0; i < node.childNodes.length; i++) walk(node.childNodes[i], fn);
    }
  }

  function removeHighlights(el) {
    const marks = el.querySelectorAll('mark');
    marks.forEach((m) => {
      const txt = document.createTextNode(m.textContent);
      m.parentNode.replaceChild(txt, m);
      m.parentNode.normalize && m.parentNode.normalize();
    });
  }

  function initContainer(container) {
    const input = container.querySelector('.table-search');
    const grid = container.querySelector('.entity-grid');
    const counter = container.querySelector('.table-results-counter');
    if (!input || !grid) return;

    const cards = () => Array.from(grid.children).filter((n) => n.nodeType === 1);

    const doFilter = (value) => {
      const q = (value || '').trim().toLowerCase();
      let visible = 0;
      cards().forEach((card) => {
        removeHighlights(card);
        const text = card.innerText || card.textContent || '';
        if (!q) {
          showCard(card);
          visible++;
          return;
        }
        if (text.toLowerCase().includes(q)) {
          showCard(card);
          try { highlightText(card, q); } catch (e) { /* swallow */ }
          visible++;
        } else {
          hideCard(card);
        }
      });
      if (counter) counter.textContent = `${visible} results found`;
    };

    const debounced = debounce((e) => doFilter(e.target.value), 200);
    input.addEventListener('input', debounced);

    // initial count
    doFilter(input.value || '');
  }

  function hideCard(card) {
    card.style.transition = 'opacity 350ms ease, transform 350ms ease, height 350ms ease';
    card.style.opacity = '0';
    card.style.transform = 'translateY(-8px)';
    card.setAttribute('aria-hidden', 'true');
    // after transition, set display: none
    requestAnimationFrame(() => {
      setTimeout(() => { card.style.display = 'none'; }, 360);
    });
  }

  function showCard(card) {
    card.style.display = '';
    // measure and then animate in
    requestAnimationFrame(() => {
      card.style.transition = 'opacity 350ms ease, transform 350ms ease';
      card.style.opacity = '1';
      card.style.transform = 'translateY(0)';
      card.removeAttribute('aria-hidden');
    });
  }

  function init() {
    document.querySelectorAll('.table-filter-wrap').forEach(initContainer);
  }

  return { init };
})();

document.addEventListener('DOMContentLoaded', () => {
  try { TableFilter.init(); } catch (e) { console.error('TableFilter init failed', e); }
});
