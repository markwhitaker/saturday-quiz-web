import SceneType from "./SceneType.js";
import Question from "./Question";

export default class Scene {
    private readonly _type: SceneType;
    private readonly _date: Date;
    private readonly _question: Question;

    private constructor(type, question, date) {
        this._type = type;
        this._date = date;
        this._question = question;
    }

    static questionsTitleScene(date) {
        return new Scene(SceneType.QUESTIONS_TITLE, null, date);
    }

    static questionScene(question) {
        return new Scene(SceneType.QUESTION, question, null);
    }

    static answersTitleScene() {
        return new Scene(SceneType.ANSWERS_TITLE, null, null);
    }

    static questionAnswerScene(question) {
        return new Scene(SceneType.QUESTION_ANSWER, question, null);
    }

    static endTitleScene() {
        return new Scene(SceneType.END_TITLE, null, null);
    }

    get type() {
        return this._type;
    }

    get date() {
        return this._date;
    }

    get question() {
        return this._question;
    }
}
