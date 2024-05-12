export default class LocalStore {
    static #KEYS = Object.freeze({
        QUIZ_DATE: "quiz-date",
        QUIZ_JSON: "quiz-json",
        SCORES: "scores"
    });

    get quizDate() {
        return localStorage.getItem(LocalStore.#KEYS.QUIZ_DATE);
    }

    set quizDate(value) {
        localStorage.setItem(LocalStore.#KEYS.QUIZ_DATE, value);
    }

    get quizJson() {
        return localStorage.getItem(LocalStore.#KEYS.QUIZ_JSON);
    }

    set quizJson(value) {
        localStorage.setItem(LocalStore.#KEYS.QUIZ_JSON, value);
    }

    get scores() {
        return localStorage.getItem(LocalStore.#KEYS.SCORES);
    }

    set scores(value) {
        localStorage.setItem(LocalStore.#KEYS.SCORES, value);
    }

    clearScores() {
        localStorage.removeItem(LocalStore.#KEYS.SCORES);
    }
}
