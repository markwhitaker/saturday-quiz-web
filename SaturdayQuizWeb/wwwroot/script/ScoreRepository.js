import LocalStore from "./LocalStore.js";
import QuestionScore from "./QuestionScore.js";

export default class ScoreRepository {
    #localStore;
    #scores;

    constructor(dependencies) {
        this.#localStore = dependencies?.localStore ?? new LocalStore();
        this.#scores = [];
    }

    getTotalScore() {
        return this.#scores.length === 0 ? 0 : this.#scores.reduce((a, b) => a + b);
    }

    hasScores() {
        return !this.#scores.every(s => s === QuestionScore.NONE);
    }

    getAllScores() {
        return this.#scores;
    }

    initialiseScores(quiz) {
        this.#scores = this.#loadScores() ?? new Array(quiz.getQuestions().length).fill(QuestionScore.NONE);
        this.#saveScores();
    }

    getScore(questionNumber) {
        return this.#scores[questionNumber - 1];
    }

    setScore(questionNumber, score) {
        this.#scores[questionNumber - 1] = score;
        this.#saveScores();
    }

    #loadScores() {
        const scoreString = this.#localStore.getScores();
        return scoreString?.split(",").map(parseFloat);
    }

    #saveScores() {
        this.#localStore.setScores(this.#scores.toString());
    }
}
