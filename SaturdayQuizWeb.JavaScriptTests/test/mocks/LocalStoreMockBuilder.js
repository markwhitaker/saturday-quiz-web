export default class LocalStoreMockBuilder {
    #mock = {
        getScores: () => {},
        setScores: () => {},

        get scores() {
            return this.getScores();
        },
        set scores(value) {
            this.setScores(value);
        }
    };

    getScores(fn) {
        this.#mock.getScores = fn;
        return this;
    }

    setScores(fn) {
        this.#mock.setScores = fn;
        return this;
    }

    build() {
        return this.#mock;
    }
}
