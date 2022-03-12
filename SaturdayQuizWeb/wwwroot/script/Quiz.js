class Quiz {
    constructor(object) {
        this.date = object.date;
        this.questions = object.questions.map(q => new Question(q));
    }
}
