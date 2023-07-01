import QuestionScore from "./QuestionScore.js";
class ScoreRepository {
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
        var _a;
        const dateString = new Date(quiz.date).toDateString();
        if (localStorage.getItem(ScoreRepository.KEY_DATE) !== dateString) {
            localStorage.removeItem(ScoreRepository.KEY_SCORES);
        }
        localStorage.setItem(ScoreRepository.KEY_DATE, dateString);
        this._scores = (_a = ScoreRepository.loadScores()) !== null && _a !== void 0 ? _a : new Array(quiz.questions.length).fill(QuestionScore.NONE);
        this.saveScores();
    }
    getScore(questionNumber) {
        return this._scores[questionNumber - 1];
    }
    setScore(questionNumber, score) {
        this._scores[questionNumber - 1] = score;
        this.saveScores();
    }
    static loadScores() {
        const scoreString = localStorage.getItem(ScoreRepository.KEY_SCORES);
        return scoreString === null || scoreString === void 0 ? void 0 : scoreString.split(",").map(parseFloat);
    }
    saveScores() {
        localStorage.setItem(ScoreRepository.KEY_SCORES, this._scores.toString());
    }
}
ScoreRepository.KEY_DATE = "date";
ScoreRepository.KEY_SCORES = "scores";
export default ScoreRepository;
//# sourceMappingURL=ScoreRepository.js.map