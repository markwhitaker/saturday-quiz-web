import QuestionType from "./QuestionType.js";

export default class Question {
    private readonly _number: number;
    private readonly _type: string;
    private readonly _question: string;
    private readonly _answer: string;
    private readonly _isWhatLinks: boolean;

    constructor(jsonObject) {
        this._number = jsonObject.number;
        this._type = jsonObject.type;
        this._question = jsonObject.question;
        this._answer = jsonObject.answer;
        this._isWhatLinks = this._type === QuestionType.WHAT_LINKS;
    }

    get number(): number {
        return this._number;
    }

    get type(): string {
        return this._type;
    }

    get question(): string {
        return this._question;
    }

    get answer(): string {
        return this._answer;
    }

    get isWhatLinks(): boolean {
        return this._isWhatLinks;
    }
}
