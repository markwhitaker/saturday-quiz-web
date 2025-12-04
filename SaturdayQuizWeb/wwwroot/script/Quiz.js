import CalendarDate from "./CalendarDate.js";
import Question from "./Question.js";

export default class Quiz {
    #date;
    #questions;

    constructor(rawQuizObject) {
        this.#date = new CalendarDate(rawQuizObject.date);
        this.#questions = rawQuizObject.questions.map(q => new Question(q));
    }

    getDate() {
        return this.#date;
    }

    getQuestions() {
        return this.#questions;
    }
}
