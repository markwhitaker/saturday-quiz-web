import { suite, test } from 'mocha';
import assert from 'assert';
import Question from '../../SaturdayQuizWeb/wwwroot/script/src/Question.js';

suite('Question', () => {
    test('GIVEN raw question with NORMAL type WHEN question is constructed THEN expected question is returned', () => {
        const expectedNumber = 1;
        const expectedType = 'NORMAL';
        const expectedQuestion = 'question';
        const expectedAnswer = 'answer';
        const expectedIsWhatLinks = false;

        const question = new Question({
            number: expectedNumber,
            type: expectedType,
            question: expectedQuestion,
            answer: expectedAnswer
        });

        assert.strictEqual(question.number, expectedNumber);
        assert.strictEqual(question.type, expectedType);
        assert.strictEqual(question.question, expectedQuestion);
        assert.strictEqual(question.answer, expectedAnswer);
        assert.strictEqual(question.isWhatLinks, expectedIsWhatLinks);
    });

    test('GIVEN raw question with WHAT_LINKS type WHEN question is constructed THEN expected question is returned', () => {
        const expectedNumber = 1;
        const expectedType = 'WHAT_LINKS';
        const expectedQuestion = 'question';
        const expectedAnswer = 'answer';
        const expectedIsWhatLinks = true;

        const question = new Question({
            number: expectedNumber,
            type: expectedType,
            question: expectedQuestion,
            answer: expectedAnswer
        });

        assert.strictEqual(question.number, expectedNumber);
        assert.strictEqual(question.type, expectedType);
        assert.strictEqual(question.question, expectedQuestion);
        assert.strictEqual(question.answer, expectedAnswer);
        assert.strictEqual(question.isWhatLinks, expectedIsWhatLinks);
    });
});
