export default class MockScoreRepositoryBuilder {
    #mock = {
        getAllScores: () => {},
        getHasScores: () => {},
        getScore: () => {},
        getTotalScore: () => {},
        initialiseScores: () => {},
        setScore: () => {},

        get allScores() {
            return this.getAllScores();
        },

        get hasScores() {
            return this.getHasScores();
        },

        get totalScore() {
            return this.getTotalScore();
        }
    }

    getAllScores(fn) {
        this.#mock.getAllScores = fn;
        return this;
    }

    getHasScores(fn) {
        this.#mock.getHasScores = fn;
        return this;
    }

    getScore(fn) {
        this.#mock.getScore = fn;
        return this;
    }

    getTotalScore(fn) {
        this.#mock.getTotalScore = fn;
        return this;
    }

    initialiseScores(fn) {
        this.#mock.initialiseScores = fn;
        return this;
    }

    setScore(fn) {
        this.#mock.setScore = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
