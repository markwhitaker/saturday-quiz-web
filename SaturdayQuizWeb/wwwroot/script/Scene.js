const SceneType = Object.freeze({
    QUESTIONS_TITLE: 0,
    QUESTION: 1,
    ANSWERS_TITLE: 2,
    QUESTION_ANSWER: 3,
    END_TITLE: 4
});

class Scene {
    constructor(type, question, date) {
        this.type = type;
        this.date = date;
        this.question = question;
    }

    static questionsTitleScene(date) {
        return new Scene(SceneType.QUESTIONS_TITLE, null, date);
    }

    static questionScene(question) {
        return new Scene(SceneType.QUESTION, question);
    }

    static answersTitleScene() {
        return new Scene(SceneType.ANSWERS_TITLE);
    }

    static questionAnswerScene(question) {
        return new Scene(SceneType.QUESTION_ANSWER, question);
    }

    static endTitleScene() {
        return new Scene(SceneType.END_TITLE);
    }
}
