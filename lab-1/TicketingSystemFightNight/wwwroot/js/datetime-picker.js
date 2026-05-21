class DateTimePicker {
    static instances = [];
    static localeData = {
        en: {
            monthNames: ['January','February','March','April','May','June','July','August','September','October','November','December'],
            dayNames: ['Mon','Tue','Wed','Thu','Fri','Sat','Sun'],
            format: 'MM/dd/yyyy hh:mm tt',
            dateFormat: 'MM/dd/yyyy',
            labelDate: 'Date',
            labelHour: 'Hour',
            labelMinute: 'Minute',
            today: 'Today',
            clear: 'Clear'
        },
        hr: {
            monthNames: ['Siječanj','Veljača','Ožujak','Travanj','Svibanj','Lipanj','Srpanj','Kolovoz','Rujan','Listopad','Studeni','Prosinac'],
            dayNames: ['Pon','Uto','Sri','Čet','Pet','Sub','Ned'],
            format: 'dd.MM.yyyy HH:mm',
            dateFormat: 'dd.MM.yyyy',
            labelDate: 'Datum',
            labelHour: 'Sat',
            labelMinute: 'Min',
            today: 'Danas',
            clear: 'Poništi'
        }
    };

    constructor(root) {
        this.root = root;
        this.input = root.querySelector('.datetime-picker-input');
        this.hiddenInput = root.querySelector('input[type="hidden"][name]');
        this.timeHiddenInput = root.querySelector('.datetime-picker-hidden-time');
        this.popup = root.querySelector('.datetime-picker-popup');
        this.monthSelect = root.querySelector('.picker-month-select');
        this.yearSelect = root.querySelector('.picker-year-select');
        this.weekdays = root.querySelector('.picker-weekdays');
        this.calendar = root.querySelector('.picker-calendar');
        this.timeSection = root.querySelector('.picker-time');
        this.prevBtn = root.querySelector('.picker-prev');
        this.nextBtn = root.querySelector('.picker-next');
        this.todayBtn = root.querySelector('.picker-today');
        this.clearBtn = root.querySelector('.picker-clear');
        this.toggleButton = root.querySelector('.datetime-picker-toggle');

        this.includeTime = root.dataset.includeTime === 'true';
        this.customFormat = root.dataset.format || '';
        this.locale = this.detectLocale();
        this.localeConfig = DateTimePicker.localeData[this.locale];
        this.selectedDate = null;
        this.visibleDate = null;
        this.activeDate = null;
        this.hour = 0;
        this.minute = 0;
        this.isOpen = false;

        this.handleDocumentClick = this.handleDocumentClick.bind(this);
        this.handleKeyDown = this.handleKeyDown.bind(this);
        this.handleMonthChange = this.handleMonthChange.bind(this);
        this.handleYearChange = this.handleYearChange.bind(this);
        this.handleTimeChange = this.handleTimeChange.bind(this);
        this.handlePrev = this.handlePrev.bind(this);
        this.handleNext = this.handleNext.bind(this);
        this.handleToday = this.handleToday.bind(this);
        this.handleClear = this.handleClear.bind(this);
        this.handleInputToggle = this.handleInputToggle.bind(this);

        this.buildPicker();
        this.initializeState();
        this.attachEvents();
        DateTimePicker.instances.push(this);
    }

    detectLocale() {
        const language = navigator.language || navigator.userLanguage || 'en';
        return language.toLowerCase().startsWith('hr') ? 'hr' : 'en';
    }

    buildPicker() {
        this.monthSelect.innerHTML = this.localeConfig.monthNames.map((month, index) => `<option value="${index}">${month}</option>`).join('');
        const currentYear = new Date().getFullYear();
        const range = [];
        for (let offset = -6; offset <= 6; offset += 1) {
            const year = currentYear + offset;
            range.push(`<option value="${year}">${year}</option>`);
        }
        this.yearSelect.innerHTML = range.join('');
        this.renderWeekdays();
        this.renderTimeSection();
    }

    initializeState() {
        const value = this.hiddenInput.value;
        if (value) {
            const parsed = this.parseIso(value);
            if (parsed) {
                this.selectedDate = new Date(parsed.getTime());
                this.activeDate = new Date(parsed.getTime());
                this.hour = parsed.getHours();
                this.minute = parsed.getMinutes();
                this.visibleDate = new Date(parsed.getTime());
            }
        }
        if (!this.selectedDate) {
            const now = new Date();
            this.hour = now.getHours();
            this.minute = now.getMinutes();
            this.activeDate = new Date(now.getFullYear(), now.getMonth(), 1);
            this.visibleDate = now;
        }
        if (!this.selectedDate) {
            this.input.value = '';
        } else {
            this.updateDisplayValue();
        }
        this.renderCalendar();
    }

    attachEvents() {
        this.input.addEventListener('click', this.handleInputToggle);
        this.input.addEventListener('focus', this.handleInputToggle);
        this.toggleButton.addEventListener('click', this.handleInputToggle);
        this.prevBtn.addEventListener('click', this.handlePrev);
        this.nextBtn.addEventListener('click', this.handleNext);
        this.monthSelect.addEventListener('change', this.handleMonthChange);
        this.yearSelect.addEventListener('change', this.handleYearChange);
        this.todayBtn.addEventListener('click', this.handleToday);
        this.clearBtn.addEventListener('click', this.handleClear);
        document.addEventListener('click', this.handleDocumentClick);
        document.addEventListener('keydown', this.handleKeyDown);
    }

    destroy() {
        this.input.removeEventListener('click', this.handleInputToggle);
        this.input.removeEventListener('focus', this.handleInputToggle);
        this.toggleButton.removeEventListener('click', this.handleInputToggle);
        this.prevBtn.removeEventListener('click', this.handlePrev);
        this.nextBtn.removeEventListener('click', this.handleNext);
        this.monthSelect.removeEventListener('change', this.handleMonthChange);
        this.yearSelect.removeEventListener('change', this.handleYearChange);
        this.todayBtn.removeEventListener('click', this.handleToday);
        this.clearBtn.removeEventListener('click', this.handleClear);
        document.removeEventListener('click', this.handleDocumentClick);
        document.removeEventListener('keydown', this.handleKeyDown);
    }

    handleInputToggle(event) {
        event.stopPropagation();
        if (this.isOpen) {
            this.close();
            return;
        }
        this.open();
    }

    handleDocumentClick(event) {
        if (!this.root.contains(event.target)) {
            this.close();
        }
    }

    handleKeyDown(event) {
        if (!this.isOpen) {
            if (document.activeElement === this.input && ['ArrowDown','ArrowUp','Enter',' '].includes(event.key)) {
                event.preventDefault();
                this.open();
            }
            return;
        }
        const navKeys = ['ArrowLeft','ArrowRight','ArrowUp','ArrowDown'];
        if (navKeys.includes(event.key)) {
            event.preventDefault();
            this.navigateByKey(event.key);
            return;
        }
        if (event.key === 'Escape') {
            this.close();
            return;
        }
        if (event.key === 'Enter') {
            event.preventDefault();
            if (this.visibleDate) {
                this.selectDate(new Date(this.visibleDate.getTime()));
            }
            return;
        }
    }

    navigateByKey(key) {
        if (!this.visibleDate) {
            this.visibleDate = new Date();
        }
        const day = this.visibleDate.getDate();
        switch (key) {
            case 'ArrowLeft': this.visibleDate.setDate(day - 1); break;
            case 'ArrowRight': this.visibleDate.setDate(day + 1); break;
            case 'ArrowUp': this.visibleDate.setDate(day - 7); break;
            case 'ArrowDown': this.visibleDate.setDate(day + 7); break;
        }
        if (this.visibleDate.getMonth() !== this.activeDate.getMonth()) {
            this.activeDate = new Date(this.visibleDate.getFullYear(), this.visibleDate.getMonth(), 1);
        }
        this.renderCalendar();
        this.focusVisibleDayButton();
    }

    handleMonthChange(event) {
        const month = parseInt(event.target.value, 10);
        this.activeDate.setMonth(month);
        this.renderCalendar();
    }

    handleYearChange(event) {
        const year = parseInt(event.target.value, 10);
        this.activeDate.setFullYear(year);
        this.renderCalendar();
    }

    handlePrev(event) {
        event.preventDefault();
        this.activeDate.setMonth(this.activeDate.getMonth() - 1);
        this.updateSelectors();
        this.renderCalendar();
    }

    handleNext(event) {
        event.preventDefault();
        this.activeDate.setMonth(this.activeDate.getMonth() + 1);
        this.updateSelectors();
        this.renderCalendar();
    }

    handleToday(event) {
        event.preventDefault();
        const now = new Date();
        this.selectDate(new Date(now.getFullYear(), now.getMonth(), now.getDate()));
        if (this.includeTime) {
            this.hour = now.getHours();
            this.minute = now.getMinutes();
        } else {
            this.hour = 0;
            this.minute = 0;
        }
        this.updateSelectors();
        if (!this.includeTime) {
            this.close();
        }
    }

    handleClear(event) {
        event.preventDefault();
        this.selectedDate = null;
        this.visibleDate = new Date();
        this.hiddenInput.value = '';
        if (this.timeHiddenInput) {
            this.timeHiddenInput.value = '';
        }
        this.input.value = '';
        this.close();
    }

    handleTimeChange(event) {
        const target = event.target;
        if (target.classList.contains('picker-hour')) {
            this.hour = parseInt(target.value, 10);
        }
        if (target.classList.contains('picker-minute')) {
            this.minute = parseInt(target.value, 10);
        }
        this.updateValueFromSelection();
    }

    open() {
        this.isOpen = true;
        this.popup.classList.add('open');
        this.popup.setAttribute('aria-hidden', 'false');
        this.root.querySelector('.datetime-picker-input').setAttribute('aria-expanded', 'true');
        this.updateSelectors();
        this.renderCalendar();
    }

    close() {
        this.isOpen = false;
        this.popup.classList.remove('open');
        this.popup.setAttribute('aria-hidden', 'true');
        this.root.querySelector('.datetime-picker-input').setAttribute('aria-expanded', 'false');
    }

    renderWeekdays() {
        this.weekdays.innerHTML = this.localeConfig.dayNames.map(day => `<div class="picker-weekday">${day}</div>`).join('');
    }

    renderTimeSection() {
        if (!this.includeTime) {
            this.timeSection.style.display = 'none';
            return;
        }
        const hourOptions = Array.from({length: 24}, (_, index) => `<option value="${index}">${String(index).padStart(2,'0')}</option>`).join('');
        const minuteOptions = Array.from({length: 60}, (_, index) => `<option value="${index}">${String(index).padStart(2,'0')}</option>`).join('');
        this.timeSection.innerHTML = `
            <div class="picker-time-row">
                <label for="${this.hiddenInput.id}-hour">${this.localeConfig.labelHour}</label>
                <select id="${this.hiddenInput.id}-hour" class="picker-hour" aria-label="Hour selection">${hourOptions}</select>
                <label for="${this.hiddenInput.id}-minute">${this.localeConfig.labelMinute}</label>
                <select id="${this.hiddenInput.id}-minute" class="picker-minute" aria-label="Minute selection">${minuteOptions}</select>
            </div>
        `;
        this.timeSection.querySelector('.picker-hour').addEventListener('change', this.handleTimeChange);
        this.timeSection.querySelector('.picker-minute').addEventListener('change', this.handleTimeChange);
    }

    renderCalendar() {
        const year = this.activeDate.getFullYear();
        const month = this.activeDate.getMonth();
        this.monthSelect.value = month;
        this.yearSelect.value = year;

        const firstDayOfMonth = new Date(year, month, 1);
        const startOffset = (firstDayOfMonth.getDay() + 6) % 7;
        const totalDays = new Date(year, month + 1, 0).getDate();
        const previousDays = new Date(year, month, 0).getDate();

        const cells = [];
        for (let dayIndex = 0; dayIndex < 42; dayIndex += 1) {
            const dayNumber = dayIndex - startOffset + 1;
            let cellDate;
            let isCurrentMonth = true;
            if (dayNumber < 1) {
                cellDate = new Date(year, month - 1, previousDays + dayNumber);
                isCurrentMonth = false;
            } else if (dayNumber > totalDays) {
                cellDate = new Date(year, month + 1, dayNumber - totalDays);
                isCurrentMonth = false;
            } else {
                cellDate = new Date(year, month, dayNumber);
            }

            const isToday = this.isSameDate(cellDate, new Date());
            const isSelected = this.selectedDate && this.isSameDate(cellDate, this.selectedDate);
            const isOtherMonth = !isCurrentMonth;
            const isPast = cellDate < new Date(new Date().setHours(0,0,0,0));

            const classes = ['picker-day'];
            if (isOtherMonth) classes.push('picker-day--muted');
            if (isSelected) classes.push('picker-day--selected');
            if (isToday) classes.push('picker-day--today');
            if (isPast) classes.push('picker-day--past');

            cells.push(`
                <button type="button" class="${classes.join(' ')}" data-date="${cellDate.getFullYear()}-${cellDate.getMonth()}-${cellDate.getDate()}" ${isOtherMonth ? 'disabled' : ''} aria-label="${this.formatCellLabel(cellDate)}">
                    ${cellDate.getDate()}
                </button>
            `);
        }

        this.calendar.innerHTML = cells.join('');
        this.calendar.querySelectorAll('.picker-day').forEach(button => button.addEventListener('click', (event) => {
            const [yearValue, monthValue, dayValue] = event.currentTarget.dataset.date.split('-').map(Number);
            this.selectDate(new Date(yearValue, monthValue, dayValue));
        }));
        this.focusVisibleDayButton();
    }

    focusVisibleDayButton() {
        if (!this.visibleDate) {
            return;
        }
        const selector = `.picker-day[data-date="${this.visibleDate.getFullYear()}-${this.visibleDate.getMonth()}-${this.visibleDate.getDate()}"]`;
        const button = this.calendar.querySelector(selector);
        if (button) {
            button.focus();
        }
    }

    formatCellLabel(date) {
        const monthName = this.localeConfig.monthNames[date.getMonth()];
        return `${this.locale === 'hr' ? 'Datum' : 'Date'} ${date.getDate()}. ${monthName} ${date.getFullYear()}`;
    }

    selectDate(date) {
        this.selectedDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        this.visibleDate = new Date(this.selectedDate.getTime());
        this.activeDate = new Date(this.selectedDate.getFullYear(), this.selectedDate.getMonth(), 1);
        this.updateValueFromSelection();
        this.updateSelectors();
        this.renderCalendar();
        if (!this.includeTime) {
            this.close();
        }
    }

    updateValueFromSelection() {
        if (!this.selectedDate) {
            return;
        }
        const valueDate = new Date(this.selectedDate.getTime());
        if (this.includeTime) {
            valueDate.setHours(this.hour, this.minute, 0, 0);
        } else {
            valueDate.setHours(0, 0, 0, 0);
        }
        this.hiddenInput.value = this.formatIso(valueDate);
        if (this.timeHiddenInput) {
            this.timeHiddenInput.value = `${String(this.hour).padStart(2,'0')}:${String(this.minute).padStart(2,'0')}:00`;
        }
        this.updateDisplayValue();
    }

    updateDisplayValue() {
        if (!this.selectedDate) {
            this.input.value = '';
            return;
        }
        const displayDate = new Date(this.selectedDate.getTime());
        displayDate.setHours(this.hour, this.minute, 0, 0);
        this.input.value = this.formatDisplay(displayDate);
    }

    updateSelectors() {
        this.monthSelect.value = this.activeDate.getMonth();
        this.yearSelect.value = this.activeDate.getFullYear();
        if (this.includeTime) {
            const hourSelect = this.timeSection.querySelector('.picker-hour');
            const minuteSelect = this.timeSection.querySelector('.picker-minute');
            if (hourSelect) hourSelect.value = String(this.hour);
            if (minuteSelect) minuteSelect.value = String(this.minute);
        }
    }

    parseIso(value) {
        const match = /^([0-9]{4})-([0-9]{2})-([0-9]{2})T([0-9]{2}):([0-9]{2})(?::([0-9]{2}))?$/.exec(value);
        if (!match) return null;
        const year = parseInt(match[1], 10);
        const month = parseInt(match[2], 10) - 1;
        const day = parseInt(match[3], 10);
        const hour = parseInt(match[4], 10);
        const minute = parseInt(match[5], 10);
        const second = match[6] ? parseInt(match[6], 10) : 0;
        return new Date(year, month, day, hour, minute, second);
    }

    formatIso(date) {
        return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2,'0')}-${String(date.getDate()).padStart(2,'0')}T${String(date.getHours()).padStart(2,'0')}:${String(date.getMinutes()).padStart(2,'0')}:${String(date.getSeconds()).padStart(2,'0')}`;
    }

    formatDisplay(date) {
        const format = this.customFormat || (this.includeTime ? this.localeConfig.format : this.localeConfig.dateFormat || 'dd.MM.yyyy');
        const tokens = {
            dd: String(date.getDate()).padStart(2,'0'),
            MM: String(date.getMonth() + 1).padStart(2,'0'),
            yyyy: date.getFullYear(),
            HH: String(date.getHours()).padStart(2,'0'),
            mm: String(date.getMinutes()).padStart(2,'0'),
            hh: String((date.getHours() % 12) || 12).padStart(2,'0'),
            tt: date.getHours() < 12 ? 'AM' : 'PM'
        };
        return Object.keys(tokens).reduce((value, token) => value.replace(new RegExp(token, 'g'), tokens[token]), format);
    }

    isSameDate(a, b) {
        return a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth() && a.getDate() === b.getDate();
    }

    static initAll() {
        document.querySelectorAll('[data-datetime-picker="true"]').forEach(root => new DateTimePicker(root));
        window.addEventListener('unload', () => {
            DateTimePicker.instances.forEach(instance => instance.destroy());
        });
    }
}

if (document.readyState !== 'loading') {
    DateTimePicker.initAll();
} else {
    document.addEventListener('DOMContentLoaded', () => DateTimePicker.initAll());
}
