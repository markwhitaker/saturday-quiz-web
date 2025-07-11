export default class MockQuizCacheBuilder {
    #mock = {
        getCachedQuiz: () => {}
    };

    getCachedQuiz(fn) {
        this.#mock.getCachedQuiz = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
