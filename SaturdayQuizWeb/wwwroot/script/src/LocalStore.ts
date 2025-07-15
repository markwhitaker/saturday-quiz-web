import CalendarDate from "./CalendarDate.js";
import LocalStorageWrapper from "./LocalStorageWrapper.js";

interface LocalStoreDependencies {
    localStorageWrapper?: LocalStorageWrapper;
}

interface RawQuiz {
    date: string;
    questions: Array<{
        number: number;
        type: string;
        question: string;
        answer: string;
    }>;
}

export default class LocalStore {
    #localStorageWrapper: LocalStorageWrapper;

    static #KEYS = Object.freeze({
        QUIZ_CACHE_HIT_TIMESTAMP: "quiz-cache-hit-timestamp",
        QUIZ_DATE: "quiz-date",
        QUIZ_JSON: "quiz-json",
        SCORES: "scores"
    });

    constructor(dependencies: LocalStoreDependencies = {}) {
        this.#localStorageWrapper = dependencies.localStorageWrapper ?? new LocalStorageWrapper();
    }

    get quizCacheHitTimestamp(): number {
        const timestampString = this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_CACHE_HIT_TIMESTAMP);
        return timestampString ? Number(timestampString) : 0;
    }

    set quizCacheHitTimestamp(timestamp: number) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_CACHE_HIT_TIMESTAMP, timestamp.toString());
    }

    get quizDate(): CalendarDate | undefined {
        const dateString = this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_DATE);
        return dateString ? new CalendarDate(dateString) : undefined;
    }

    set quizDate(calendarDate: CalendarDate) {
        const storedCalendarDate = this.quizDate;
        if (!storedCalendarDate || !calendarDate.equals(storedCalendarDate)) {
            this.#localStorageWrapper.removeItem(LocalStore.#KEYS.SCORES);
            this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_DATE, calendarDate.toString());
        }
    }

    get quiz(): RawQuiz | null {
        const quizJson = this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_JSON);
        return quizJson ? JSON.parse(quizJson) : null;
    }

    set quiz(json: RawQuiz) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_JSON, JSON.stringify(json));
    }

    clearQuiz(): void {
        this.#localStorageWrapper.removeItem(LocalStore.#KEYS.QUIZ_JSON);
    }

    get scores(): string | null {
        return this.#localStorageWrapper.getItem(LocalStore.#KEYS.SCORES);
    }

    set scores(value: string) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.SCORES, value);
    }
}
