import {expect, test} from '@jest/globals';
import Question from "../../SaturdayQuizWeb/wwwroot/script/Question.js";

test("GIVEN object with type 'NORMAL' WHEN Question is constructed THEN all fields are as expected", () => {
    const rawQuestion = {
        number: 1,
        type: "NORMAL",
        question: "question",
        answer: "answer"
    };

    const question = new Question(rawQuestion);

    expect(question.number).toBe(1);
    expect(question.type).toBe("NORMAL");
    expect(question.question).toBe("question");
    expect(question.answer).toBe("answer");
    expect(question.isWhatLinks).toBe(false);
});

test("GIVEN object with type 'WHAT_LINKS' WHEN Question is constructed THEN all fields are as expected", () => {
    const rawQuestion = {
        number: 1,
        type: "WHAT_LINKS",
        question: "question",
        answer: "answer"
    };

    const question = new Question(rawQuestion);

    expect(question.number).toBe(1);
    expect(question.type).toBe("WHAT_LINKS");
    expect(question.question).toBe("question");
    expect(question.answer).toBe("answer");
    expect(question.isWhatLinks).toBe(true);
});
