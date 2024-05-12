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
            localStorage.removeItem(LocalStore.#KEYS.SCORES);
            localStorage.setItem(LocalStore.#KEYS.QUIZ_DATE, calendarDate.toString());
        }
    }

    get quiz() {
        return JSON.parse(localStorage.getItem(LocalStore.#KEYS.QUIZ_JSON));
    }

    set quiz(json) {
        localStorage.setItem(LocalStore.#KEYS.QUIZ_JSON, JSON.stringify(json));
    }

    clearQuiz() {
        localStorage.removeItem(LocalStore.#KEYS.QUIZ_JSON);
    }

    get scores() {
        return localStorage.getItem(LocalStore.#KEYS.SCORES);
    }

    set scores(value) {
        localStorage.setItem(LocalStore.#KEYS.SCORES, value);
    }
}
