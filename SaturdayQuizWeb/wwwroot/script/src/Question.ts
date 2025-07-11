interface RawQuestionObject {
    number: number;
    type: string;
    question: string;
    answer: string;
}

export default class Question {
    static readonly Type = Object.freeze({
        NORMAL: 'NORMAL',
        WHAT_LINKS: 'WHAT_LINKS'
    } as const);

    #number: number;
    #type: string;
    #question: string;
    #answer: string;

    constructor(object: RawQuestionObject) {
        this.#number = object.number;
        this.#type = object.type;
        this.#question = object.question;
        this.#answer = object.answer;
    }

    get number(): number {
        return this.#number;
    }

    get type(): string {
        return this.#type;
    }

    get question(): string {
        return this.#question;
    }

    get answer(): string {
        return this.#answer;
    }

    get isWhatLinks(): boolean {
        return this.#type === Question.Type.WHAT_LINKS;
    }
}
