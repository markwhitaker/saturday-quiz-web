export default class Scene {
    static #privateToken = {};

    static Type = Object.freeze({
        QUESTIONS_TITLE: 0,
        QUESTION: 1,
        ANSWERS_TITLE: 2,
        QUESTION_ANSWER: 3,
        END_TITLE: 4
    });

    constructor(type, question, date, token) {
        if (token !== Scene.#privateToken) {
            throw new Error("Scene constructor is private");
        }
        this.type = type;
        this.date = date;
        this.question = question;
    }

    static questionsTitleScene(date) {
        return new Scene(Scene.Type.QUESTIONS_TITLE, null, date, Scene.#privateToken);
    }

    static questionScene(question) {
        return new Scene(Scene.Type.QUESTION, question, null, Scene.#privateToken);
    }

    static answersTitleScene() {
        return new Scene(Scene.Type.ANSWERS_TITLE, null, null, Scene.#privateToken);
    }

    static questionAnswerScene(question) {
        return new Scene(Scene.Type.QUESTION_ANSWER, question, null, Scene.#privateToken);
    }

    static endTitleScene() {
        return new Scene(Scene.Type.END_TITLE, null, null, Scene.#privateToken);
    }
}
