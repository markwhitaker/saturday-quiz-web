import CalendarDate from "./CalendarDate.js";
import FetchWrapper from "./FetchWrapper.js";
import LocalStore from "./LocalStore.js";
import Quiz from "./Quiz.js";

export default class QuizRepository {
    #fetchWrapper;
    #localStore;
    #quizEndpoint;

    constructor(fetchWrapper, localStore) {
        this.#fetchWrapper = fetchWrapper ?? new FetchWrapper();
        this.#localStore = localStore ?? new LocalStore();
        this.#quizEndpoint = Object.freeze('api/quiz');
    }

    async loadLatestQuiz() {
        const quizJson = this.#getCachedQuiz() ?? await this.#downloadQuiz();
        const quiz = new Quiz(quizJson);
        this.#localStore.quizDate = quiz.date;
        return quiz;
    }

    async #downloadQuiz() {
        const response = await this.#fetchWrapper.fetch(this.#quizEndpoint);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${this.#quizEndpoint}. ${response.status}: ${response.statusText}`);
        }
        const rawQuizObject = await response.json();
        this.#localStore.quiz = rawQuizObject;
        return rawQuizObject;
    }

    #getCachedQuiz() {
        const cachedRawQuizObject = this.#localStore.quiz;
        const quizDate = this.#localStore.quizDate;
        if (cachedRawQuizObject && quizDate && QuizRepository.#isFewerThan7DaysAgo(quizDate)) {
            return new Quiz(cachedRawQuizObject);
        }

        this.#localStore.clearQuiz();
        return undefined;
    }

    static #isFewerThan7DaysAgo(calendarDate) {
        return calendarDate.isGreaterThan(new CalendarDate().subtractDays(7));
    }
}
