export default class MockDateWrapperBuilder {
    #mock = {
        getNow: () => {}
    };

    getNow(fn) {
        this.#mock.getNow = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
