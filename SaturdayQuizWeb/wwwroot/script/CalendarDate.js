export default class CalendarDate {
    #date;

    constructor(date) {
        this.#date = date ? new Date(date) : new Date();
        this.#date.setHours(0, 0, 0, 0);
    }

    toString() {
        return this.#date.toDateString();
    }

    toLocaleString(locales, options) {
        return this.#date.toLocaleDateString(locales, options);
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
