export default class QuizRepositoryMockBuilder {
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
