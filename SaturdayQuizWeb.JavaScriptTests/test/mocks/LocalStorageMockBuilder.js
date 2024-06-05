export default class LocalStorageMockBuilder {
    #mock = {
        getQuizDate: () => {},
        setQuizDate: () => {},

        getQuiz: () => {},
        setQuiz: () => {},
        clearQuiz: () => {},

        getScores: () => {},
        setScores: () => {},

        get quizDate() {
            return this.getQuizDate();
        },
        set quizDate(value) {
            this.setQuizDate(value);
        },
        get quiz() {
            return this.getQuiz();
        },
        set quiz(value) {
            this.setQuiz(value);
        },
        get scores() {
            return this.getScores();
        },
        set scores(value) {
            this.setScores(value);
        }
    };

    getQuizDate(fn) {
        this.#mock.getQuizDate = fn;
        return this;
    }

    setQuizDate(fn) {
        this.#mock.setQuizDate = fn;
        return this;
    }

    getQuiz(fn) {
        this.#mock.getQuiz = fn;
        return this;
    }

    setQuiz(fn) {
        this.#mock.setQuiz = fn;
        return this;
    }

    clearQuiz(fn) {
        this.#mock.clearQuiz = fn;
        return this;
    }

    getScores(fn) {
        this.#mock.getScores = fn;
        return this;
    }

    setScores(fn) {
        this.#mock.setScores = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
