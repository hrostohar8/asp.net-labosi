/* Client-side validation enhancements for jQuery Validate */
;(function($) {
    if (!$) return;

    function validateAutocompleteField($input) {
        const $root = $input.closest('.autocomplete-dropdown');
        if (!$root.length) return true;

        const $hidden = $root.find('.autocomplete-hidden');
        const $error = $root.find('.autocomplete-error');
        const required = $input.prop('required');
        const value = $hidden.val();
        const valid = !required || (value && value.toString().trim().length > 0);

        if (!valid) {
            $input.addClass('is-invalid');
            $error.text('Odaberite stavku s popisa.').show();
        } else {
            $input.removeClass('is-invalid');
            $error.hide();
        }

        return valid;
    }

    let initAttempts = 0;

    function configureValidation() {
        if (!$.validator || !$.validator.unobtrusive) {
            if (initAttempts < 20) {
                initAttempts += 1;
                setTimeout(configureValidation, 100);
            }
            return;
        }

        $.validator.setDefaults({
            onfocusout: function(element) {
                this.element(element);
                validateAutocompleteField($(element));
            },
            onkeyup: false,
            onclick: false
        });

        $('form').each(function() {
            const $form = $(this);
            if (!$form.data('validator')) {
                $form.validate();
            }

            $form.find('input[data-val="true"], select[data-val="true"], textarea[data-val="true"]').on('blur', function() {
                if ($form.data('validator')) {
                    $form.data('validator').element(this);
                }
            });

            $form.find('.autocomplete-text').on('blur', function() {
                validateAutocompleteField($(this));
            });
        });
    }

    $(function() {
        configureValidation();
    });
})(window.jQuery);
