import CalendarDate from "./CalendarDate.js";
import LocalStorageWrapper from "./LocalStorageWrapper.js";

export default class LocalStore {
    #localStorageWrapper;

    static #KEYS = Object.freeze({
        QUIZ_CACHE_HIT_TIMESTAMP: "quiz-cache-hit-timestamp",
        QUIZ_DATE: "quiz-date",
        QUIZ_JSON: "quiz-json",
        SCORES: "scores"
    });

    constructor(dependencies) {
        this.#localStorageWrapper = dependencies?.localStorageWrapper ?? new LocalStorageWrapper();
    }

    getQuizCacheHitTimestamp() {
        const timestampString = this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_CACHE_HIT_TIMESTAMP);
        return timestampString ? Number(timestampString) : 0;
    }

    setQuizCacheHitTimestamp(timestamp) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_CACHE_HIT_TIMESTAMP, timestamp.toString());
    }

    getQuizDate() {
        const dateString = this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_DATE);
        return dateString ? new CalendarDate(dateString) : undefined;
    }

    setQuizDate(calendarDate) {
        const storedCalendarDate = this.getQuizDate();
        if (!calendarDate.equals(storedCalendarDate)) {
            this.#localStorageWrapper.removeItem(LocalStore.#KEYS.SCORES);
            this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_DATE, calendarDate.toString());
        }
    }

    getQuiz() {
        return JSON.parse(this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_JSON));
    }

    setQuiz(json) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_JSON, JSON.stringify(json));
    }

    clearQuiz() {
        this.#localStorageWrapper.removeItem(LocalStore.#KEYS.QUIZ_JSON);
    }

    getScores() {
        return this.#localStorageWrapper.getItem(LocalStore.#KEYS.SCORES);
    }

    setScores(value) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.SCORES, value);
    }
}
