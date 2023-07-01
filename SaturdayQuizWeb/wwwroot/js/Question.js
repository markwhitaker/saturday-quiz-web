import QuestionType from "./QuestionType.js";
export default class Question {
    constructor(jsonObject) {
        this._number = jsonObject.number;
        this._type = jsonObject.type;
        this._question = jsonObject.question;
        this._answer = jsonObject.answer;
        this._isWhatLinks = this._type === QuestionType.WHAT_LINKS;
    }
    get number() {
        return this._number;
    }
    get type() {
        return this._type;
    }
    get question() {
        return this._question;
    }
    get answer() {
        return this._answer;
    }
    get isWhatLinks() {
        return this._isWhatLinks;
    }
}
//# sourceMappingURL=Question.js.map