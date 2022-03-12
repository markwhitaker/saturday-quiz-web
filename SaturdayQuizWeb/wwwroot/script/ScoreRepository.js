const KEY_DATE = "date";
const KEY_SCORES = "scores";

const QuestionScore = Object.freeze({
    NONE: 0,
    HALF: 0.5,
    FULL: 1
});

class ScoreRepository {
    _scores = []

    get totalScore() {
        return this._scores.reduce((a, b) => a + b);
    }

    get hasScores() {
        return !this._scores.every(s => s === QuestionScore.NONE);
    }

    initialiseScores(quiz) {
        let dateString = new Date(quiz.date).toDateString();

        if (localStorage.getItem(KEY_DATE) !== dateString) {
            localStorage.removeItem(KEY_SCORES)
        }

        localStorage.setItem(KEY_DATE, dateString);

        this._scores = ScoreRepository._loadScores() ?? new Array(quiz.questions.length).fill(QuestionScore.NONE);
        this._saveScores();
    }

    getScore(questionNumber) {
        return this._scores[questionNumber - 1];
    }

    setScore(questionNumber, score) {
        this._scores[questionNumber - 1] = score;
        this._saveScores();
    }

    static _loadScores() {
        let scoreString = localStorage.getItem(KEY_SCORES);
        return scoreString?.split(",").map(s => parseFloat(s));
    }

    _saveScores() {
        localStorage.setItem(KEY_SCORES, this._scores.toString());
    }
}
