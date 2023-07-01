import Question from "./Question.js";
export default class Quiz {
    constructor(jsonObject) {
        this._date = new Date(jsonObject.date);
        this._questions = jsonObject.questions.map(q => new Question(q));
    }
    get date() {
        return this._date;
    }
    get questions() {
        return this._questions;
    }
}
//# sourceMappingURL=Quiz.js.map