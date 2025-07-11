import LocalStore from "./LocalStore.js";
import QuestionScore from "./QuestionScore.js";
import Quiz from "./Quiz.js";

interface ScoreRepositoryDependencies {
    localStore?: LocalStore;
}

export default class ScoreRepository {
    #localStore: LocalStore;
    #scores: number[];

    constructor(dependencies: ScoreRepositoryDependencies = {}) {
        this.#localStore = dependencies.localStore ?? new LocalStore();
        this.#scores = [];
    }

    get totalScore(): number {
        return this.#scores.length === 0 ? 0 : this.#scores.reduce((a, b) => a + b);
    }

    get hasScores(): boolean {
        return !this.#scores.every(s => s === QuestionScore.NONE);
    }

    get allScores(): number[] {
        return this.#scores;
    }

    initialiseScores(quiz: Quiz): void {
        this.#scores = this.#loadScores() ?? new Array(quiz.questions.length).fill(QuestionScore.NONE);
        this.#saveScores();
    }

    getScore(questionNumber: number): number {
        return this.#scores[questionNumber - 1] ?? QuestionScore.NONE;
    }

    setScore(questionNumber: number, score: number): void {
        this.#scores[questionNumber - 1] = score;
        this.#saveScores();
    }

    #loadScores(): number[] | undefined {
        const scoreString = this.#localStore.scores;
        return scoreString?.split(",").map(parseFloat);
    }

    #saveScores(): void {
        this.#localStore.scores = this.#scores.toString();
    }
}
