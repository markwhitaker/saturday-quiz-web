export default class Test {
    private readonly x : number = 0;

    private constructor(x: number) {
        this.x = x;
    }

    static construct(x: number): Test {
        return new Test(x * 10);
    }

    getX(): number {
        return this.x;
    }
}
