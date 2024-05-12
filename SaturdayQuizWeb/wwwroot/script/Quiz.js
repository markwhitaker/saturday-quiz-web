import CalendarDate from "./CalendarDate.js";
import Question from "./Question.js";

export default class Quiz {
    constructor(object) {
        this.date = new CalendarDate(object.date);
        this.questions = object.questions.map(q => new Question(q));
    }
}
