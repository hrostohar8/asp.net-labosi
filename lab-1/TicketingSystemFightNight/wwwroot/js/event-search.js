;(function($){
    if (!$) return;
    const debounce = (fn, wait) => { let t; return function(...args){ clearTimeout(t); t = setTimeout(()=>fn.apply(this,args), wait); }; };

    const escapeHtml = (s) => String(s||'')
        .replace(/&/g, '&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;').replace(/'/g,'&#039;');

    const buildCard = (e) => {
        return `<article class="entity-card">
            <h3>${escapeHtml(e.Text)}</h3>
            <div class="entity-props">
                <p><strong>Id:</strong> ${escapeHtml(e.Id)}</p>
                <p><strong>Organizacija:</strong> ${escapeHtml(e.Organization)}</p>
                <p><strong>Grad:</strong> ${escapeHtml(e.City)}</p>
                <p><strong>Datum:</strong> ${escapeHtml(e.Date)}</p>
                <p><strong>Vrijeme:</strong> ${escapeHtml(e.Time)}</p>
                <p><strong>Arena:</strong> ${escapeHtml(e.Venue)}</p>
                <p><strong>Opis:</strong> ${escapeHtml(e.Description)}</p>
                <p><strong>Prodano karata:</strong> ${escapeHtml(e.TicketsSold)}</p>
                <p><strong>Broj mečeva:</strong> ${escapeHtml(e.MatchesCount)}</p>
            </div>
            <div class="entity-actions">
                <a class="btn" href="/Event/Details/${encodeURIComponent(e.Id)}">Detalji</a>
                <a class="btn" href="/Event/Edit/${encodeURIComponent(e.Id)}">Uredi</a>
                <form action="/Event/Delete/${encodeURIComponent(e.Id)}" method="post" class="inline-form" onsubmit="return confirm('Jeste li sigurni da želite obrisati ovaj događaj?');"><button type="submit" class="btn btn-danger">Obriši</button></form>
            </div>
        </article>`;
    };

    const render = ($grid, $counter, items) => {
        if (!items || !items.length) { $grid.html('<div class="no-results-message">Nema događaja.</div>'); $counter.text('0 results found'); return; }
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
