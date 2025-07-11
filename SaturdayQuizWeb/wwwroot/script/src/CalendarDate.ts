import TimeSpan from "./TimeSpan.js";

export default class CalendarDate {
    #date: Date;

    constructor(date?: string | Date) {
        this.#date = date ? new Date(date) : new Date();
        this.#date.setHours(0, 0, 0, 0);
    }

    toString(): string {
        return this.#date.toDateString();
    }

    toDisplayString(): string {
        return this.#date.toLocaleDateString('en-GB', {
            day: 'numeric',
            month: 'long',
            year: 'numeric'
        });
    }

    subtractDays(days: number): CalendarDate {
        const newDate = new Date(this.#date);
        newDate.setDate(newDate.getDate() - days);
        return new CalendarDate(newDate);
    }

    diff(other: CalendarDate): TimeSpan {
        return new TimeSpan(Math.abs(this.#date.getTime() - other.#date.getTime()));
    }

    equals(other: CalendarDate): boolean {
        return other != null && this.#date.getTime() === other.#date.getTime();
    }
}
