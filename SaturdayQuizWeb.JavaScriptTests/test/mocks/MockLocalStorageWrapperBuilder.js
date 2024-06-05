export default class MockLocalStorageWrapperBuilder {
    #mock = {
        setItem: () => {},
        getItem: () => {},
        removeItem: () => {}
    }

    setItem(fn) {
        this.#mock.setItem = fn;
        return this;
    }

    getItem(fn) {
        this.#mock.getItem = fn;
        return this;
    }

    removeItem(fn) {
        this.#mock.removeItem = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
