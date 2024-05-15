import CalendarDate from "./CalendarDate.js";
import Question from "./Question.js";

export default class Quiz {
    constructor(rawQuizObject) {
        this.date = new CalendarDate(rawQuizObject.date);
        this.questions = rawQuizObject.questions.map(q => new Question(q));
    }
}
