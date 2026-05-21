/* form-enhancements.js
   Conditional fields, age calc, currency formatting, progress indicator, char counters
*/
const FormEnhancements = (() => {
  const q = (sel, ctx=document) => ctx.querySelector(sel);
  const qa = (sel, ctx=document) => Array.from(ctx.querySelectorAll(sel));

  function calcAge(dob) {
    if (!dob) return '';
    try {
      const b = new Date(dob);
      const now = new Date();
      let age = now.getFullYear() - b.getFullYear();
      const m = now.getMonth() - b.getMonth();
      if (m < 0 || (m === 0 && now.getDate() < b.getDate())) age--;
      return age;
    } catch (e) { return ''; }
  }

  function bindAgeField(form) {
    const dob = q('input[type="date"][name$="DateOfBirth"]', form);
    const out = document.createElement('div'); out.className = 'age-output';
    if (!dob) return;
    dob.parentNode.insertBefore(out, dob.nextSibling);
    const update = () => { const v = calcAge(dob.value); out.textContent = v ? `Age: ${v}` : ''; };
    dob.addEventListener('input', update);
    update();
  }

  function bindCurrencyInputs(form) {
    qa('input[data-currency]', form).forEach((inp) => {
      // keep input value numeric (so model binding works) and show a formatted preview next to it
      let preview = inp.nextElementSibling;
      if (!preview || !preview.classList || !preview.classList.contains('currency-preview')) {
        preview = document.createElement('div');
        preview.className = 'currency-preview';
        preview.style.marginTop = '6px';
        preview.style.color = 'var(--silver)';
        inp.parentNode.insertBefore(preview, inp.nextSibling);
      }
      const format = () => {
        const raw = inp.value.replace(/[^0-9.]/g, '');
        if (!raw) { preview.textContent = ''; return; }
        try { preview.textContent = Number(raw).toLocaleString(undefined, { style: 'currency', currency: 'USD' }); } catch (e) { preview.textContent = raw; }
      };
      inp.addEventListener('input', format);
      format();
    });
  }

  function bindConditionalSections(form) {
    // Example: on Match/Create weight class selection shows extra fields
    const weight = q('select[name$="WeightClassId"]', form);
    if (weight) {
      const section = form.querySelector('.conditional-weight-details');
      const toggle = () => { if (!section) return; section.style.display = (weight.value ? '' : 'none'); };
      weight.addEventListener('change', () => { requestAnimationFrame(toggle); });
      toggle();
    }

    const championship = q('input[type="checkbox"][name$="Championship"]', form);
    if (championship) {
      const champSection = form.querySelector('.conditional-championship');
      const toggle = () => { if (!champSection) return; champSection.style.display = championship.checked ? '' : 'none'; };
      championship.addEventListener('change', () => { requestAnimationFrame(toggle); });
      toggle();
    }
  }

  function bindCharCounters(form) {
    qa('textarea[maxlength], input[maxlength]', form).forEach((el) => {
      const max = Number(el.getAttribute('maxlength')) || 0;
      const out = document.createElement('div'); out.className = 'char-counter';
      const update = () => { out.textContent = `${el.value.length}/${max}`; };
      el.parentNode.insertBefore(out, el.nextSibling);
      el.addEventListener('input', update);
      update();
    });
  }

  function bindProgressIndicator(form) {
    // Create progress container
    const bar = document.createElement('div'); bar.className = 'form-progress';
    const inner = document.createElement('div'); inner.className = 'form-progress-inner';
    const text = document.createElement('div'); text.className = 'form-progress-text'; text.style.marginTop = '6px'; text.style.color = 'var(--silver)';
    bar.appendChild(inner);
    form.insertBefore(bar, form.firstChild);
    form.insertBefore(text, bar.nextSibling);

    // If form has explicit .form-step elements, use that behaviour, otherwise fall back
    const steps = qa('.form-step', form);

    const isVisible = (el) => {
      if (!el) return false;
      if (el.offsetParent === null) return false; // covers display:none
      const style = getComputedStyle(el);
      if (style.visibility === 'hidden' || style.display === 'none' || style.opacity === '0') return false;
      // ensure ancestors are visible
      let p = el.parentElement;
      while (p) {
        const s = getComputedStyle(p);
        if (s.display === 'none' || s.visibility === 'hidden') return false;
        p = p.parentElement;
      }
      return true;
    };

    const getRequiredVisibleFields = () => {
      const all = Array.from(form.querySelectorAll('input[required], select[required], textarea[required]'));
      return all.filter(el => isVisible(el));
    };

    const isFieldFilled = (el) => {
      if (!el) return false;
      const tag = el.tagName.toLowerCase();
      const type = (el.getAttribute('type') || '').toLowerCase();
      if (tag === 'select') return el.selectedIndex > 0 || (el.value && el.value.trim() !== '');
      if (type === 'checkbox' || type === 'radio') return el.checked;
      if (type === 'file') return el.files && el.files.length > 0;
      return (el.value || '').toString().trim() !== '';
    };

    const updateFallback = () => {
      const requiredVisible = getRequiredVisibleFields();
      const total = requiredVisible.length;
      const filled = requiredVisible.filter(isFieldFilled).length;
      const pct = total === 0 ? 0 : Math.round((filled / total) * 100);
      inner.style.width = `${pct}%`;
      text.textContent = total === 0 ? 'No required fields' : `${filled}/${total} required fields filled (${pct}%)`;
    };

    const updateSteps = () => {
      const total = steps.length;
      const visible = steps.filter(s => s.style.display !== 'none').length;
      const pct = total === 0 ? 0 : Math.round((visible / total) * 100);
      inner.style.width = `${pct}%`;
      text.textContent = `${visible}/${total} steps visible (${pct}%)`;
    };

    const update = () => {
      try {
        if (steps.length) updateSteps(); else updateFallback();
      } catch (e) { console.error('progress update failed', e); }
    };

    // Bind input/change to the form to update in real-time
    form.addEventListener('input', update);
    form.addEventListener('change', update);

    // initial
    update();

    // If steps are present, observe style changes on steps
    if (steps.length) {
      const mo = new MutationObserver(update);
      steps.forEach(s => mo.observe(s, { attributes: true, attributeFilter: ['style'] }));
    }
  }

  function init() {
    document.querySelectorAll('form.enhanced').forEach((form) => {
      try {
        bindAgeField(form);
        bindCurrencyInputs(form);
        bindConditionalSections(form);
        bindCharCounters(form);
        bindProgressIndicator(form);
      } catch (e) { console.error('FormEnhancements error', e); }
    });
  }

  return { init };
})();

document.addEventListener('DOMContentLoaded', () => { try { FormEnhancements.init(); } catch(e){console.error(e);} });
