export default class Logger {
    static #isInBrowser(): boolean {
        return typeof window !== 'undefined';
    }

    static log(message: string): void {
        if (Logger.#isInBrowser()) {
            console.log(message);
        }
    }

    static debug(message: string): void {
        if (Logger.#isInBrowser()) {
            console.debug(message);
        }
    }
}
