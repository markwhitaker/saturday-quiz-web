const SceneType = Object.freeze({
    QUESTIONS_TITLE: 0,
    QUESTION: 1,
    ANSWERS_TITLE: 2,
    QUESTION_ANSWER: 3,
    END_TITLE: 4
});

class Scene {
    static #privateToken = {};

    constructor(type, question, date, token) {
        if (token !== Scene.#privateToken) {
            throw new Error("Scene constructor is private");
        }
        this.type = type;
        this.date = date;
        this.question = question;
    }

    static questionsTitleScene(date) {
        return new Scene(SceneType.QUESTIONS_TITLE, null, date, this.#privateToken);
    }

    static questionScene(question) {
        return new Scene(SceneType.QUESTION, question, null, this.#privateToken);
    }

    static answersTitleScene() {
        return new Scene(SceneType.ANSWERS_TITLE, null, null, this.#privateToken);
    }

    static questionAnswerScene(question) {
        return new Scene(SceneType.QUESTION_ANSWER, question, null, this.#privateToken);
    }

    static endTitleScene() {
        return new Scene(SceneType.END_TITLE, null, null, this.#privateToken);
    }
}
