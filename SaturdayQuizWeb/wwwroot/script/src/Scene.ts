import Question from "./Question.js";
import CalendarDate from "./CalendarDate.js";

export default class Scene {
    static #privateToken = {};

    static readonly Type = Object.freeze({
        QUESTIONS_TITLE: 0,
        QUESTION: 1,
        ANSWERS_TITLE: 2,
        QUESTION_ANSWER: 3,
        END_TITLE: 4
    } as const);

    type: number;
    question: Question | null;
    date: CalendarDate | null;

    private constructor(type: number, question: Question | null, date: CalendarDate | null, token: object) {
        if (token !== Scene.#privateToken) {
            throw new Error("Scene constructor is private");
        }
        this.type = type;
        this.date = date;
        this.question = question;
    }

    static questionsTitleScene(date: CalendarDate): Scene {
        return new Scene(Scene.Type.QUESTIONS_TITLE, null, date, Scene.#privateToken);
    }

    static questionScene(question: Question): Scene {
        return new Scene(Scene.Type.QUESTION, question, null, Scene.#privateToken);
    }

    static answersTitleScene(): Scene {
        return new Scene(Scene.Type.ANSWERS_TITLE, null, null, Scene.#privateToken);
    }

    static questionAnswerScene(question: Question): Scene {
        return new Scene(Scene.Type.QUESTION_ANSWER, question, null, Scene.#privateToken);
    }

    static endTitleScene(): Scene {
        return new Scene(Scene.Type.END_TITLE, null, null, Scene.#privateToken);
    }
}
