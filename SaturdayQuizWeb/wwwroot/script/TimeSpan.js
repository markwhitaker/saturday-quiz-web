const MILLISECONDS_PER_SECOND = 1000;
const MILLISECONDS_PER_MINUTE = 60 * MILLISECONDS_PER_SECOND;
const MILLISECONDS_PER_HOUR = 60 * MILLISECONDS_PER_MINUTE;
const MILLISECONDS_PER_DAY = 24 * MILLISECONDS_PER_HOUR;

export default class TimeSpan {
    #milliseconds;

    constructor(milliseconds) {
        this.#milliseconds = milliseconds;
    }

    get milliseconds() {
        return this.#milliseconds;
    }

    static fromSeconds = seconds => new TimeSpan(seconds * MILLISECONDS_PER_SECOND);

    static fromMinutes = minutes => new TimeSpan(minutes * MILLISECONDS_PER_MINUTE);

    static fromHours = hours => new TimeSpan(hours * MILLISECONDS_PER_HOUR);

    static fromDays = days => new TimeSpan(days * MILLISECONDS_PER_DAY);

    isLessThan = other => other && this.#milliseconds < other.#milliseconds;

    toString = () => {
        const milliseconds = this.#milliseconds % MILLISECONDS_PER_SECOND;
        const seconds = Math.floor((this.#milliseconds % MILLISECONDS_PER_MINUTE) / MILLISECONDS_PER_SECOND);
        const minutes = Math.floor((this.#milliseconds % MILLISECONDS_PER_HOUR) / MILLISECONDS_PER_MINUTE);
        const hours = Math.floor((this.#milliseconds % MILLISECONDS_PER_DAY) / MILLISECONDS_PER_HOUR);
        const days = Math.floor(this.#milliseconds / MILLISECONDS_PER_DAY);

        const parts = [];

        if (days > 0) {
            parts.push(`${days}d`);
        }
        if (hours > 0) {
            parts.push(`${hours}h`);
        }
        if (minutes > 0) {
            parts.push(`${minutes}m`);
        }
        if (seconds > 0) {
            parts.push(`${seconds}s`);
        }
        if (milliseconds > 0) {
            parts.push(`${milliseconds}ms`);
        }

        return parts.join(' ');
    }

    equals = other => other && this.#milliseconds === other.#milliseconds;
}
