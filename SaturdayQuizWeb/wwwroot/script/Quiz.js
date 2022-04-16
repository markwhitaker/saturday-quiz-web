class Quiz {
    constructor(object) {
        this.date = new Date(object.date);
        this.questions = object.questions.map(q => new Question(q));
    }
}
