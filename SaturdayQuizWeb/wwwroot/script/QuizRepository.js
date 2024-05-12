import CalendarDate from "./CalendarDate.js";
import LocalStore from "./LocalStore.js";
import Quiz from "./Quiz.js";

export default class QuizRepository {
    #localStore = new LocalStore();
    #quizEndpoint = Object.freeze("api/quiz");

    async loadLatestQuiz() {
        const quizJson = this.#getCachedQuizJson() ?? await this.#downloadQuizJson();
        const quiz = new Quiz(quizJson);
        this.#localStore.quizDate = quiz.date;
        return quiz;
    }

    async #downloadQuizJson() {
        const response = await fetch(this.#quizEndpoint);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${this.#quizEndpoint}. ${response.status}: ${response.statusText}`);
        }
        const rawQuizObject = await response.json();
        this.#localStore.quiz = rawQuizObject;
        return rawQuizObject;
    }

    #getCachedQuizJson() {
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
