export default class Logger {
    static #isInBrowser = () => typeof window !== 'undefined';

    static log = message => Logger.#isInBrowser() && console.log(message);

    static debug = message => Logger.#isInBrowser() && console.debug(message);
}
