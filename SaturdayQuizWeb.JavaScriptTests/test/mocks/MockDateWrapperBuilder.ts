export default class MockDateWrapperBuilder {
    #mock = {
        getNow: () => {},

        get now() {
            return this.getNow();
        }
    };

    getNow(fn) {
        this.#mock.getNow = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
