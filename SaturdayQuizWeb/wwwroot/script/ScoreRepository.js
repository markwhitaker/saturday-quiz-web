﻿const KEY_DATE = "DATE";
const KEY_SCORES = "SCORES";

const QuestionScore = Object.freeze({
    NONE: 0,
    HALF: 0.5,
    FULL: 1
});

class ScoreRepository {
    #scores = []

    get totalScore() {
        return this.#scores.reduce((a, b) => a + b);
    }

    get hasScores() {
        return this.#scores.every(s => s !== QuestionScore.NONE);
    }

    initialiseScores(quiz) {
        let dateString = new Date(quiz.date).toDateString();

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
        let scoreString = localStorage.getItem(KEY_SCORES);
        return scoreString?.split(",").map(s => parseFloat(s));
    }

    #saveScores() {
        localStorage.setItem(KEY_SCORES, this.#scores.toString());
    }
}
