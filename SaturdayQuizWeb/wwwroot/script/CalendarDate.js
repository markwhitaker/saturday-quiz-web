export default class CalendarDate {
    #date;

    constructor(date) {
        this.#date = date ? new Date(date) : new Date();
        this.#date.setHours(0, 0, 0, 0);
    }

    toString() {
        return this.#date.toDateString();
    }

    toDisplayString() {
        return this.#date.toLocaleDateString('en-GB', {
            day: 'numeric',
            month: 'long',
            year: 'numeric'
        });
    }

    subtractDays(days) {
        const newDate = new Date(this.#date);
        newDate.setDate(newDate.getDate() - days);
        return new CalendarDate(newDate);
    }

    isGreaterThan(other) {
        return this.#date > other.#date;
    }

    equals(other) {
        return other && this.#date.getTime() === other.#date.getTime();
    }
}
