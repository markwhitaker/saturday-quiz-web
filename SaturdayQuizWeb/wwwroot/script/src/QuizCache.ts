import CalendarDate from "./CalendarDate.js";
import DateWrapper from "./DateWrapper.js";
import Logger from "./Logger.js";
import LocalStore from "./LocalStore.js";
import TimeSpan from "./TimeSpan.js";

interface QuizCacheDependencies {
    dateWrapper?: DateWrapper;
    localStore?: LocalStore;
}

interface RawQuizObject {
    date: string;
    questions: Array<{
        number: number;
        type: string;
        question: string;
        answer: string;
    }>;
}

export default class QuizCache {
    #dateWrapper: DateWrapper;
    #localStore: LocalStore;

    static readonly skipCacheIfReloadedWithin = Object.freeze(TimeSpan.fromSeconds(5));

    constructor(dependencies: QuizCacheDependencies = {}) {
        this.#dateWrapper = dependencies.dateWrapper ?? new DateWrapper();
        this.#localStore = dependencies.localStore ?? new LocalStore();
    }

    getCachedQuiz(): RawQuizObject | undefined {
        const cachedRawQuizObject = this.#localStore.quiz;
        const quizDate = this.#localStore.quizDate;
        const msSinceLastCacheHit = this.#dateWrapper.now - this.#localStore.quizCacheHitTimestamp;
        const shouldReturnFromCache =
            !!cachedRawQuizObject &&
            !!quizDate &&
            QuizCache.#isFewerThan7DaysAgo(quizDate) &&
            msSinceLastCacheHit > QuizCache.skipCacheIfReloadedWithin.milliseconds;

        if (shouldReturnFromCache) {
            this.#localStore.quizCacheHitTimestamp = this.#dateWrapper.now;
            Logger.debug("Returning cached quiz...");
            return cachedRawQuizObject;
        }

        this.#localStore.clearQuiz();
        return undefined;
    }

    static #isFewerThan7DaysAgo(calendarDate: CalendarDate): boolean {
        return calendarDate.diff(new CalendarDate()).isLessThan(TimeSpan.fromDays(7));
    }
}
