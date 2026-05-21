/* match-stats.js
   Fetch match data and render charts using Chart.js if available.
*/
const MatchStats = (() => {
  async function fetchJson(url) {
    try {
      const res = await fetch(url, { headers: { 'Accept': 'application/json' } });
      if (!res.ok) throw new Error(`Fetch failed: ${res.status}`);
      return await res.json();
    } catch (e) { console.error('match-stats fetch error', e); return null; }
  }

  function renderComparisonChart(ctx, fighters) {
    if (typeof Chart === 'undefined') return;
    const labels = fighters.map(f => f.name);
    const data = {
      labels,
      datasets: [
        { label: 'Wins', data: fighters.map(f => f.wins || 0), backgroundColor: 'rgba(54,162,235,0.8)' },
        { label: 'Losses', data: fighters.map(f => f.losses || 0), backgroundColor: 'rgba(255,99,132,0.8)' }
      ]
    };
    new Chart(ctx, { type: 'bar', data, options: { responsive: true, animation: { duration: 600 }, plugins: { tooltip: { enabled: true } } } });
  }

  function renderPie(ctx, distribution) {
    if (typeof Chart === 'undefined') return;
    const data = { labels: distribution.map(d=>d.label), datasets: [{ data: distribution.map(d=>d.value), backgroundColor: ['#ff6384','#36a2eb','#ffcd56','#4bc0c0','#9966ff'] }] };
    new Chart(ctx, { type: 'pie', data, options: { responsive: true, animation: { duration: 700 } } });
  }

  async function init() {
    try {
      const root = document.getElementById('match-stats');
      if (!root) return;
      const matchId = root.dataset.matchId;
      if (!matchId) return;
      const data = await fetchJson(`/Match/ApiStats/${matchId}`);
      if (!data) return;
      // fighters: [{name,height,weight,reach,wins,losses}, ...]
      const cmp = root.querySelector('.stats-comparison canvas');
      const pie = root.querySelector('.stats-pie canvas');
      renderComparisonChart(cmp.getContext('2d'), data.fighters);
      renderPie(pie.getContext('2d'), data.weightDistribution || []);
      // wins/losses simple text
      const wl = root.querySelector('.stats-wl');
      if (wl && data.fighters) {
        wl.innerHTML = data.fighters.map(f => `<div class="wl-item">${f.name}: ${f.wins}W / ${f.losses}L</div>`).join('');
      }
    } catch (e) { console.error('MatchStats init error', e); }
  }

  return { init };
})();

document.addEventListener('DOMContentLoaded', () => { try { MatchStats.init(); } catch(e){console.error(e);} });
