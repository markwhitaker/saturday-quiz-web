class Quiz {
    constructor(quizObject) {
        this.date = quizObject.date;
        this.questions = quizObject.questions.map(q => new Question(q));
    }
}
