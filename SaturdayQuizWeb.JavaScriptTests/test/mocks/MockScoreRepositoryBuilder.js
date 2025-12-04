export default class MockScoreRepositoryBuilder {
    #mock = {
        getAllScores: () => {},
        hasScores: () => {},
        getScore: () => {},
        getTotalScore: () => {},
        initialiseScores: () => {},
        setScore: () => {}
    }

    getAllScores(fn) {
        this.#mock.getAllScores = fn;
        return this;
    }

    hasScores(fn) {
        this.#mock.hasScores = fn;
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
