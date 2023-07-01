export default class Question {
    static Type = Object.freeze({
        NORMAL: 'NORMAL',
        WHAT_LINKS: 'WHAT_LINKS'
    });

    constructor(object) {
        this.number = object.number;
        this.type = object.type;
        this.question = object.question;
        this.answer = object.answer;
        this.isWhatLinks = this.type === Question.Type.WHAT_LINKS;
    }
}
