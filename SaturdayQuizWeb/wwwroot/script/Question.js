import QuestionType from "../js/QuestionType.js";

export default class Question {
    constructor(object) {
        this.number = object.number;
        this.type = object.type;
        this.question = object.question;
        this.answer = object.answer;
        this.isWhatLinks = this.type === QuestionType.WHAT_LINKS;
    }
}
