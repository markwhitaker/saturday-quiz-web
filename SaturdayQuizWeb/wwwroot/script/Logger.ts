export default class Logger {
    private static isInBrowser: () => boolean = () => typeof window !== 'undefined';

    static log: (message: String) => void = (message: String) => {
        if (Logger.isInBrowser()) {
            console.log(message);
        }
    }

    static debug: (message: String) => void = (message: String) => {
        if (Logger.isInBrowser()) {
            console.debug(message);
        }
    }
}
