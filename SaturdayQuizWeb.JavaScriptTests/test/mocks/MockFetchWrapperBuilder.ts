export default class MockFetchWrapperBuilder {
    #mock = {
        fetch: async () => {}
    }

    fetch(fn) {
        this.#mock.fetch = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
};
