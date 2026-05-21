/* Autocomplete AJAX control (requires jQuery) */
;(function($){
    if (!$) return;

    function debounce(fn, wait) {
        let t;
        return function(){
            const ctx = this, args = arguments;
            clearTimeout(t);
            t = setTimeout(() => fn.apply(ctx, args), wait);
        };
    }

    function normalizeItem(item) {
        return {
            id: item.Id ?? item.id,
            text: item.Text ?? item.text,
            description: item.Description ?? item.description
        };
    }

    function createItemHtml(item) {
        const normalized = normalizeItem(item);
        return '<div class="autocomplete-item" data-id="'+ normalized.id +'">'
            + '<div class="autocomplete-item-title">'+ normalized.text +'</div>'
            + (normalized.description ? '<div class="autocomplete-item-desc">'+ normalized.description +'</div>' : '')
            +'</div>';
    }

    function bindAutocomplete($root) {
        const ajaxUrl = $root.data('ajax-url');
        const fieldName = $root.data('field-name');
        const extraParamName = $root.data('extra-param-name') || null;
        const extraParamSelector = $root.data('extra-param-selector') || null;

        const $text = $root.find('.autocomplete-text');
        const $hidden = $root.find('.autocomplete-hidden');
        const $list = $root.find('.autocomplete-dropdown-list');
        const $error = $root.find('.autocomplete-error');
        const $clear = $root.find('.btn-clear');
        const isRequired = $text.prop('required') || false;

        let items = [];
        let highlighted = -1;

        function showLoading() {
            $list.html('<div class="autocomplete-loading">Loading…</div>').show();
        }

        function showNoResults(){
            $list.html('<div class="autocomplete-no-results">Nema rezultata</div>').show();
        }

        function showError(msg){
            $error.text(msg).show();
        }

        function clearError(){ $error.hide(); }

        function renderResults(data){
            items = data || [];
            if (!items.length) { showNoResults(); return; }
            const html = items.slice(0,10).map(createItemHtml).join('');
            $list.html(html).show();
            highlighted = -1;
        }

        function selectIndex(i){
            if (i < 0 || i >= items.length) return;
            const it = normalizeItem(items[i]);
            $text.val(it.text);
            $hidden.val(it.id);
            $text.removeClass('is-invalid');
            clearError();
            $list.hide();
        }

        function clearSelection(){
            $text.val('');
            $hidden.val('');
            $text.removeClass('is-invalid');
            clearError();
            $list.hide();
        }

        const doSearch = debounce(function(){
            const term = $text.val().trim();
            if (term.length < 2) { $list.hide(); return; }
            clearError();
            showLoading();
            const data = { term: term };
            if (extraParamName && extraParamSelector) {
                const $sel = $(extraParamSelector);
                if ($sel && $sel.length) data[extraParamName] = $sel.val();
            }
            $.ajax({
                url: ajaxUrl,
                method: 'GET',
                data: data,
                dataType: 'json'
            }).done(function(resp){
                renderResults(resp || []);
            }).fail(function(){
                showError('Došlo je do pogreške pri pretrazi');
            });
        }, 300);

        $text.on('input', function(){
            $hidden.val('');
            $text.removeClass('is-invalid');
            clearError();
            doSearch();
        });

        $text.on('keydown', function(e){
            if (!$list.is(':visible')) return;
            if (e.key === 'ArrowDown'){
                e.preventDefault(); highlighted = Math.min(highlighted + 1, items.length - 1);
                $list.find('.autocomplete-item').removeClass('active').eq(highlighted).addClass('active');
            } else if (e.key === 'ArrowUp'){
                e.preventDefault(); highlighted = Math.max(highlighted -1, 0);
                $list.find('.autocomplete-item').removeClass('active').eq(highlighted).addClass('active');
            } else if (e.key === 'Enter'){
                e.preventDefault(); if (highlighted >=0) selectIndex(highlighted);
            } else if (e.key === 'Escape'){
                $list.hide();
            }
        });

        $list.on('click', '.autocomplete-item', function(){
            const id = $(this).data('id');
            const idx = $list.find('.autocomplete-item').index(this);
            if (idx >=0) selectIndex(idx);
        });

        $clear.on('click', function(){ clearSelection(); });

        $(document).on('click', function(e){
            if (!$.contains($root[0], e.target)) {
                $list.hide();
            }
        });

        // if the parent form is submitted, validate hidden input when required
        $root.closest('form').on('submit', function(e){
            if (isRequired && (!$hidden.val() || $hidden.val().trim() === '')){
                e.preventDefault();
                $text.addClass('is-invalid');
                showError('Ovo polje je obavezno. Odaberite stavku s popisa.');
                $text.focus();
                return false;
            }
        });
    }

    $(function(){
        $('.autocomplete-dropdown').each(function(){ bindAutocomplete($(this)); });
    });
})(window.jQuery);
