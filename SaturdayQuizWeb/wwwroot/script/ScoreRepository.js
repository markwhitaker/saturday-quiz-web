import LocalStore from "./LocalStore.js";
import QuestionScore from "./QuestionScore.js";

export default class ScoreRepository {
    #localStore = new LocalStore();
    #scores = []

    get totalScore() {
        return this.#scores.reduce((a, b) => a + b);
    }

    get hasScores() {
        return !this.#scores.every(s => s === QuestionScore.NONE);
    }

    get allScores() {
        return this.#scores;
    }

    initialiseScores(quiz) {
        const quizDateString = new Date(quiz.date).toDateString();

        if (this.#localStore.quizDate !== quizDateString) {
            this.#localStore.clearScores();
        }

        this.#localStore.quizDate = quizDateString;

        this.#scores = this.#loadScores() ?? new Array(quiz.questions.length).fill(QuestionScore.NONE);
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
        const scoreString = this.#localStore.scores;
        return scoreString?.split(",").map(parseFloat);
    }

    #saveScores() {
        this.#localStore.scores = this.#scores.toString();
    }
}
