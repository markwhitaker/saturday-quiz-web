import QuestionScore from "./QuestionScore.js";

export default class ScoreRepository {
    private static readonly KEY_DATE = "date";
    private static readonly KEY_SCORES = "scores";

    private _scores: number[];

    get totalScore() {
        return this._scores.reduce((a, b) => a + b);
    }

    get hasScores() {
        return !this._scores.every(s => s === QuestionScore.NONE);
    }

    get allScores() {
        return this._scores;
    }

    initialiseScores(quiz) {
        const dateString = new Date(quiz.date).toDateString();

        if (localStorage.getItem(ScoreRepository.KEY_DATE) !== dateString) {
            localStorage.removeItem(ScoreRepository.KEY_SCORES)
        }

        localStorage.setItem(ScoreRepository.KEY_DATE, dateString);

        this._scores = ScoreRepository.loadScores() ?? new Array(quiz.questions.length).fill(QuestionScore.NONE);
        this.saveScores();
    }

    getScore(questionNumber) {
        return this._scores[questionNumber - 1];
    }

    setScore(questionNumber, score) {
        this._scores[questionNumber - 1] = score;
        this.saveScores();
    }

    private static loadScores() {
        const scoreString = localStorage.getItem(ScoreRepository.KEY_SCORES);
        return scoreString?.split(",").map(parseFloat);
    }

    private saveScores() {
        localStorage.setItem(ScoreRepository.KEY_SCORES, this._scores.toString());
    }
}
