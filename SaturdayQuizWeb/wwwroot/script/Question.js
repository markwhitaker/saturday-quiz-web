export default class Question {
    static Type = Object.freeze({
        NORMAL: 'NORMAL',
        WHAT_LINKS: 'WHAT_LINKS'
    });

    #number;
    #type;
    #question;
    #answer;

    constructor(object) {
        this.#number = object.number;
        this.#type = object.type;
        this.#question = object.question;
        this.#answer = object.answer;
    }

    get number() {
        return this.#number;
    }

    get type() {
        return this.#type;
    }

    get question() {
        return this.#question;
    }

    get answer() {
        return this.#answer;
    }

    get isWhatLinks() {
        return this.#type === Question.Type.WHAT_LINKS;
    }
}
