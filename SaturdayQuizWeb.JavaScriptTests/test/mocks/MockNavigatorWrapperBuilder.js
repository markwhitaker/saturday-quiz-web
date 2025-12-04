export default class MockNavigatorWrapperBuilder {
    #mock = {
        isShareSupported: () => {},
        share: () => {}
    }

    isShareSupported(fn) {
        this.#mock.isShareSupported = fn;
        return this;
    }

    share(fn) {
        this.#mock.share = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
