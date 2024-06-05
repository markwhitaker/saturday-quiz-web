export default class MockQuizRepositoryBuilder {
    #mock = {
        loadLatestQuiz: async () => {}
    }

    loadLatestQuiz(fn) {
        this.#mock.loadLatestQuiz = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
};
