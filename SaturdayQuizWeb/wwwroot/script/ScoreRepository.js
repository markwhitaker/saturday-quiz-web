import QuestionScore from "../js/QuestionScore.js";

const KEY_DATE = "date";
const KEY_SCORES = "scores";

export default class ScoreRepository {
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
        const dateString = new Date(quiz.date).toDateString();

        if (localStorage.getItem(KEY_DATE) !== dateString) {
            localStorage.removeItem(KEY_SCORES)
        }

        localStorage.setItem(KEY_DATE, dateString);

        this.#scores = ScoreRepository.#loadScores() ?? new Array(quiz.questions.length).fill(QuestionScore.NONE);
        this.#saveScores();
    }

    getScore(questionNumber) {
        return this.#scores[questionNumber - 1];
    }

    setScore(questionNumber, score) {
        this.#scores[questionNumber - 1] = score;
        this.#saveScores();
    }

    static #loadScores() {
        const scoreString = localStorage.getItem(KEY_SCORES);
        return scoreString?.split(",").map(parseFloat);
    }

    #saveScores() {
        localStorage.setItem(KEY_SCORES, this.#scores.toString());
    }
}
