import LocalStorageWrapper from "../../../SaturdayQuizWeb/wwwroot/script/LocalStorageWrapper.js";

export default class LocalStoreWrapperMockBuilder {
    #setItem = () => {};
    #getItem = () => {};
    #removeItem = () => {};

    setItem(fn) {
        this.#setItem = fn;
        return this;
    }

    getItem(fn) {
        this.#getItem = fn;
        return this;
    }

    removeItem(fn) {
        this.#removeItem = fn;
        return this;
    }

    build() {
        let mock = new LocalStorageWrapper();
        mock.setItem = this.#setItem;
        mock.getItem = this.#getItem;
        mock.removeItem = this.#removeItem;
        return mock;
    }
}
