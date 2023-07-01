import Question from "./Question.js";

export default class Quiz {
    private readonly _date: Date;
    private readonly _questions: Question[];

    constructor(jsonObject) {
        this._date = new Date(jsonObject.date);
        this._questions = jsonObject.questions.map(q => new Question(q));
    }

    get date(): Date {
        return this._date;
    }

    get questions(): Question[] {
        return this._questions;
    }
}
