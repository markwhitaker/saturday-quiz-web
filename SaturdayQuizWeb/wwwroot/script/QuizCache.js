import CalendarDate from "./CalendarDate.js";
import DateWrapper from "./DateWrapper.js";
import Logger from "./Logger.js";
import LocalStore from "./LocalStore.js";
import TimeSpan from "./TimeSpan.js";

export default class QuizCache {
    #dateWrapper;
    #localStore;

    static skipCacheIfReloadedWithin = Object.freeze(TimeSpan.fromSeconds(5));

    constructor(dependencies) {
        this.#dateWrapper = dependencies?.dateWrapper ?? new DateWrapper();
        this.#localStore = dependencies?.localStore ?? new LocalStore();
    }

    getCachedQuiz() {
        const cachedRawQuizObject = this.#localStore.getQuiz();
        const quizDate = this.#localStore.getQuizDate();
        const msSinceLastCacheHit = this.#dateWrapper.getNow() - this.#localStore.getQuizCacheHitTimestamp();
        const shouldReturnFromCache =
            !!cachedRawQuizObject &&
            !!quizDate &&
            QuizCache.#isFewerThan7DaysAgo(quizDate) &&
            msSinceLastCacheHit > QuizCache.skipCacheIfReloadedWithin.getMilliseconds();

        if (shouldReturnFromCache) {
            this.#localStore.setQuizCacheHitTimestamp(this.#dateWrapper.getNow());
            Logger.debug("Returning cached quiz...")
            return cachedRawQuizObject;
        }

        this.#localStore.clearQuiz();
        return undefined;
    }

    static #isFewerThan7DaysAgo(calendarDate) {
        return calendarDate.diff(new CalendarDate()).isLessThan(TimeSpan.fromDays(7));
    }
}
