import CalendarDate from "./CalendarDate.js";
import DateWrapper from "./DateWrapper.js";
import LocalStore from "./LocalStore.js";

export default class QuizCache {
    #dateWrapper;
    #localStore;

    static #skipCacheTimeoutMs = 10 * 1000;

    constructor(dependencies) {
        this.#dateWrapper = dependencies?.dateWrapper ?? new DateWrapper();
        this.#localStore = dependencies?.localStore ?? new LocalStore();
    }

    getCachedQuiz() {
        const cachedRawQuizObject = this.#localStore.quiz;
        const quizDate = this.#localStore.quizDate;
        const msSinceLastCacheHit = this.#dateWrapper.now - this.#localStore.quizCacheHitTimestamp;
        if (cachedRawQuizObject && quizDate && QuizCache.#isFewerThan7DaysAgo(quizDate) && msSinceLastCacheHit > QuizCache.#skipCacheTimeoutMs) {
            this.#localStore.quizCacheHitTimestamp = this.#dateWrapper.now;
            console.debug("Returning cached quiz...")
            return cachedRawQuizObject;
        }

        this.#localStore.clearQuiz();
        return undefined;
    }

    static #isFewerThan7DaysAgo(calendarDate) {
        return calendarDate.isGreaterThan(new CalendarDate().subtractDays(7));
    }
}
