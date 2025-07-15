import FetchWrapper from "./FetchWrapper.js";
import LocalStore from "./LocalStore.js";
import Logger from "./Logger.js";
import Quiz from "./Quiz.js";
import QuizCache from "./QuizCache.js";

interface QuizRepositoryDependencies {
    fetchWrapper?: FetchWrapper;
    localStore?: LocalStore;
    quizCache?: QuizCache;
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

export default class QuizRepository {
    #fetchWrapper: FetchWrapper;
    #localStore: LocalStore;
    #quizCache: QuizCache;
    #quizEndpoint: string;

    constructor(dependencies: QuizRepositoryDependencies = {}) {
        this.#fetchWrapper = dependencies.fetchWrapper ?? new FetchWrapper();
        this.#localStore = dependencies.localStore ?? new LocalStore();
        this.#quizCache = dependencies.quizCache ?? new QuizCache();
        this.#quizEndpoint = Object.freeze('api/quiz');
    }

    async loadLatestQuiz(): Promise<Quiz> {
        const quizJson = this.#quizCache.getCachedQuiz() ?? await this.#downloadQuiz();
        const quiz = new Quiz(quizJson);
        this.#localStore.quizDate = quiz.date;
        return quiz;
    }

    async #downloadQuiz(): Promise<RawQuizObject> {
        Logger.debug("Downloading quiz...");
        const response = await this.#fetchWrapper.fetch(this.#quizEndpoint);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${this.#quizEndpoint}. ${response.status}: ${response.statusText}`);
        }
        const rawQuizObject: RawQuizObject = await response.json();
        this.#localStore.quiz = rawQuizObject;
        return rawQuizObject;
    }
}
