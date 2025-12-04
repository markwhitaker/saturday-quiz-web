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

    getNumber() {
        return this.#number;
    }

    getType() {
        return this.#type;
    }

    getQuestion() {
        return this.#question;
    }

    getAnswer() {
        return this.#answer;
    }

    isWhatLinks() {
        return this.#type === Question.Type.WHAT_LINKS;
    }
}
