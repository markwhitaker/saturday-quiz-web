export default class MockNavigatorWrapperBuilder {
    #mock = {
        shareSupported: () => {},
        share: () => {},

        get isShareSupported() {
            return this.shareSupported();
        }
    }

    isShareSupported(fn) {
        this.#mock.shareSupported = fn;
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
