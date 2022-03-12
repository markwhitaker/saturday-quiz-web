const SceneType = Object.freeze({
    QUESTIONS_TITLE: 0,
    QUESTION: 1,
    ANSWERS_TITLE: 2,
    QUESTION_ANSWER: 3,
    END_TITLE: 4
});

class Scene {
    constructor(type, date, question) {
        this.type = type;
        this.date = date;
        this.question = question;
    }
}
