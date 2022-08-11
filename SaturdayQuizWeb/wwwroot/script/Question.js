"use strict";

const QuestionType = Object.freeze({
    NORMAL: 'NORMAL',
    WHAT_LINKS: 'WHAT_LINKS'
});

class Question {
    constructor(object) {
        this.number = object.number;
        this.type = object.type;
        this.question = object.question;
        this.answer = object.answer;
    }
}
