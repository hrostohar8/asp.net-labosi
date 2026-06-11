;(function($){
    if (!$) return;

    const debounce = (fn, wait) => {
        let timer;
        return function(...args) {
            clearTimeout(timer);
            timer = setTimeout(() => fn.apply(this, args), wait);
        };
    };

    const normalizeProp = (item, name) => {
        return item[name] ?? item[name[0].toLowerCase() + name.slice(1)];
    };

    const buildCardHtml = (fighter) => {
        const imageUrl = normalizeProp(fighter, 'ImageUrl') || '/images/fighter-placeholder.svg';
        const name = normalizeProp(fighter, 'Text') || normalizeProp(fighter, 'Name') || 'Borac';
        const nickname = normalizeProp(fighter, 'Nickname') || '';
        const weightClass = normalizeProp(fighter, 'WeightClass') || '';
        const organization = normalizeProp(fighter, 'Organization') || '';
        const country = normalizeProp(fighter, 'Country') || '';
        const wins = normalizeProp(fighter, 'Wins');
        const losses = normalizeProp(fighter, 'Losses');

        return `<article class="entity-card">
            <img src="${imageUrl}" alt="${escapeHtml(name)}" class="entity-card-image" />
            <h3>${escapeHtml(name)}</h3>
            <div class="entity-props">
                <p><strong>Id:</strong> ${escapeHtml(normalizeProp(fighter, 'Id').toString())}</p>
                <p><strong>Nadimak:</strong> ${escapeHtml(nickname)}</p>
                <p><strong>Težinska klasa:</strong> ${escapeHtml(weightClass)}</p>
                <p><strong>Organizacija:</strong> ${escapeHtml(organization)}</p>
                <p><strong>Država:</strong> ${escapeHtml(country)}</p>
                <p><strong>Pobjede:</strong> ${escapeHtml(wins?.toString() ?? '0')}</p>
                <p><strong>Porazi:</strong> ${escapeHtml(losses?.toString() ?? '0')}</p>
            </div>
            <div class="entity-actions">
                <a class="btn" href="/Fighter/Details/${encodeURIComponent(normalizeProp(fighter, 'Id'))}">Detalji</a>
                <a class="btn" href="/Fighter/Edit/${encodeURIComponent(normalizeProp(fighter, 'Id'))}">Uredi</a>
                <form asp-controller="Fighter" asp-action="Delete" action="/Fighter/Delete/${encodeURIComponent(normalizeProp(fighter, 'Id'))}" method="post" class="inline-form" onsubmit="return confirm('Jeste li sigurni da želite obrisati ovog borca?');">
                    <button type="submit" class="btn btn-danger">Obriši</button>
                </form>
            </div>
        </article>`;
    };

    const escapeHtml = (text) => {
        return String(text)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    };

    const renderResults = ($grid, $counter, items) => {
        if (!items || !items.length) {
            $grid.html('<div class="no-results-message">Nije pronađen nijedan borac.</div>');
            $counter.text('0 results found');
            return;
        }

        const html = items.map(buildCardHtml).join('');
        $grid.html(html);
        $counter.text(`${items.length} results found`);
    };

    const init = () => {
        $('.table-filter-wrap[data-ajax-url]').each(function() {
            const $wrap = $(this);
            const ajaxUrl = $wrap.data('ajax-url');
            const $input = $wrap.find('.table-search');
            const $grid = $wrap.find('.entity-grid');
            const $counter = $wrap.find('.table-results-counter');

            if (!$input.length || !$grid.length || !ajaxUrl) return;

            const originalHtml = $grid.html();
            const originalCounterText = $counter.text();

            const loadResults = debounce((term) => {
                const q = String(term || '').trim();
                if (!q) {
                    $grid.html(originalHtml);
                    $counter.text(originalCounterText);
                    return;
                }

                $.ajax({
                    url: ajaxUrl,
                    method: 'GET',
                    data: { term: q },
                    dataType: 'json'
                }).done(function(resp) {
                    renderResults($grid, $counter, resp || []);
                }).fail(function() {
                    $grid.html('<div class="no-results-message">Greška pri pretrazi. Pokušajte ponovno.</div>');
                    $counter.text('0 results found');
                });
            }, 250);

            $input.on('input', function() {
                loadResults($(this).val());
            });
        });
    };

    $(document).ready(init);
})(window.jQuery);
