import CalendarDate from "./CalendarDate.js";
import LocalStorageWrapper from "./LocalStorageWrapper.js";

export default class LocalStore {
    #localStorageWrapper;

    static #KEYS = Object.freeze({
        QUIZ_DATE: "quiz-date",
        QUIZ_JSON: "quiz-json",
        SCORES: "scores"
    });

    constructor(dependencies) {
        this.#localStorageWrapper = dependencies?.localStorageWrapper ?? new LocalStorageWrapper();
    }

    get quizDate() {
        const dateString = this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_DATE);
        return dateString ? new CalendarDate(dateString) : undefined;
    }

    set quizDate(calendarDate) {
        const storedCalendarDate = this.quizDate;
        if (!calendarDate.equals(storedCalendarDate)) {
            this.#localStorageWrapper.removeItem(LocalStore.#KEYS.SCORES);
            this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_DATE, calendarDate.toString());
        }
    }

    get quiz() {
        return JSON.parse(this.#localStorageWrapper.getItem(LocalStore.#KEYS.QUIZ_JSON));
    }

    set quiz(json) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.QUIZ_JSON, JSON.stringify(json));
    }

    clearQuiz() {
        this.#localStorageWrapper.removeItem(LocalStore.#KEYS.QUIZ_JSON);
    }

    get scores() {
        return this.#localStorageWrapper.getItem(LocalStore.#KEYS.SCORES);
    }

    set scores(value) {
        this.#localStorageWrapper.setItem(LocalStore.#KEYS.SCORES, value);
    }
}
