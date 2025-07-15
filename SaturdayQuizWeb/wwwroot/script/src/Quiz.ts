import CalendarDate from "./CalendarDate.js";
import Question from "./Question.js";

interface RawQuizObject {
    date: string;
    questions: Array<{
        number: number;
        type: string;
        question: string;
        answer: string;
    }>;
}

export default class Quiz {
    #date: CalendarDate;
    #questions: Question[];

    constructor(rawQuizObject: RawQuizObject) {
        this.#date = new CalendarDate(rawQuizObject.date);
        this.#questions = rawQuizObject.questions.map(q => new Question(q));
    }

    get date(): CalendarDate {
        return this.#date;
    }

    get questions(): Question[] {
        return this.#questions;
    }
}
