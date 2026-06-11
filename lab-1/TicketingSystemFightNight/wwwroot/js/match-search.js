;(function($){
    if (!$) return;
    const debounce = (fn, wait) => { let t; return function(...args){ clearTimeout(t); t = setTimeout(()=>fn.apply(this,args), wait); }; };

    const escapeHtml = (s) => String(s||'')
        .replace(/&/g, '&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;').replace(/'/g,'&#039;');

    const buildCard = (m) => {
        return `<article class="entity-card">
            <h3>${escapeHtml(m.Title)}</h3>
            <div class="entity-props">
                <p><strong>Id:</strong> ${escapeHtml(m.Id)}</p>
                <p><strong>Borac 1:</strong> ${escapeHtml(m.Fighter1)}</p>
                <p><strong>Borac 2:</strong> ${escapeHtml(m.Fighter2)}</p>
                <p><strong>Težinska klasa:</strong> ${escapeHtml(m.WeightClass)}</p>
                <p><strong>Događaj:</strong> ${escapeHtml(m.Event)}</p>
                <p><strong>Runde:</strong> ${escapeHtml(m.RoundLimit)}</p>
                <p><strong>Šampionski:</strong> ${m.Championship ? 'Da' : 'Ne'}</p>
                <p><strong>Sudac:</strong> ${escapeHtml(m.Referee)}</p>
                <p><strong>Status:</strong> ${escapeHtml(m.Status)}</p>
            </div>
            <div class="entity-actions">
                <a class="btn" href="/Match/Details/${encodeURIComponent(m.Id)}">Detalji</a>
                <a class="btn" href="/Match/Edit/${encodeURIComponent(m.Id)}">Uredi</a>
                <form action="/Match/Delete/${encodeURIComponent(m.Id)}" method="post" class="inline-form" onsubmit="return confirm('Jeste li sigurni da želite obrisati ovaj meč?');"><button type="submit" class="btn btn-danger">Obriši</button></form>
            </div>
        </article>`;
    };

    const render = ($grid, $counter, items) => {
        if (!items || !items.length) { $grid.html('<div class="no-results-message">Nema mečeva.</div>'); $counter.text('0 results found'); return; }
        $grid.html(items.map(buildCard).join(''));
        $counter.text(items.length + ' results found');
    };

    const init = () => {
        $('.table-filter-wrap[data-ajax-url]').each(function(){
            const $wrap = $(this);
            const ajaxUrl = $wrap.data('ajax-url');
            const $input = $wrap.find('.table-search');
            const $grid = $wrap.find('.entity-grid');
            const $counter = $wrap.find('.table-results-counter');
            if (!$input.length || !$grid.length || !ajaxUrl) return;

            const originalHtml = $grid.html();
            const originalCounterText = $counter.text();

            const load = debounce((term) => {
                const query = String(term || '').trim();
                if (!query) {
                    $grid.html(originalHtml);
                    $counter.text(originalCounterText);
                    return;
                }

                $.ajax({ url: ajaxUrl, method: 'GET', data: { term: query }, dataType: 'json' })
                .done(function(resp){ render($grid, $counter, resp || []); })
                .fail(function(){ $grid.html('<div class="no-results-message">Greška pri pretrazi.</div>'); $counter.text('0 results found'); });
            }, 250);

            $input.on('input', function(){ load($(this).val()); });
        });
    };

    $(document).ready(init);
})(window.jQuery);
