export default class Test {
    constructor(x) {
        this.x = 0;
        this.x = x;
    }
    static construct(x) {
        return new Test(x * 10);
    }
    getX() {
        return this.x;
    }
}
//# sourceMappingURL=test.js.map