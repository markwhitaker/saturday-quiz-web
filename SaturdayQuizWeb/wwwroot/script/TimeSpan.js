export default class TimeSpan {
    #milliseconds;

    constructor(milliseconds) {
        this.#milliseconds = milliseconds;
    }

    get milliseconds() {
        return this.#milliseconds;
    }

    static fromSeconds = seconds => new TimeSpan(seconds * 1000);

    toString = () => {
        const milliseconds = (this.#milliseconds % 1000);
        const seconds = Math.floor(this.#milliseconds / 1000);
        const parts = [];

        if (seconds > 0) {
            parts.push(`${seconds}s`);
        }
        if (milliseconds > 0) {
            parts.push(`${milliseconds}ms`);
        }

        return parts.join(' ');
    }
}
