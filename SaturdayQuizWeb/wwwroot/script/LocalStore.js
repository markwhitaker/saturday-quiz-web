import CalendarDate from "./CalendarDate.js";

export default class LocalStore {
    static #KEYS = Object.freeze({
        QUIZ_DATE: "quiz-date",
        QUIZ_JSON: "quiz-json",
        SCORES: "scores"
    });

    get quizDate() {
        const dateString = localStorage.getItem(LocalStore.#KEYS.QUIZ_DATE);
        return dateString ? new CalendarDate(dateString) : undefined;
    }

    set quizDate(calendarDate) {
        const storedCalendarDate = this.quizDate;
        if (!calendarDate.equals(storedCalendarDate)) {
            this.#clearScores();
            localStorage.setItem(LocalStore.#KEYS.QUIZ_DATE, calendarDate.toString());
        }
    }

    get quizJson() {
        return JSON.parse(localStorage.getItem(LocalStore.#KEYS.QUIZ_JSON));
    }

    set quizJson(json) {
        localStorage.setItem(LocalStore.#KEYS.QUIZ_JSON, JSON.stringify(json));
    }

    clearQuizJson() {
        localStorage.removeItem(LocalStore.#KEYS.QUIZ_JSON);
    }

    get scores() {
        return localStorage.getItem(LocalStore.#KEYS.SCORES);
    }

    set scores(value) {
        localStorage.setItem(LocalStore.#KEYS.SCORES, value);
    }

    #clearScores() {
        localStorage.removeItem(LocalStore.#KEYS.SCORES);
    }
}
