import { describe, test, expect } from 'bun:test';
import Question from '../../SaturdayQuizWeb/wwwroot/script/Question.js';

describe('Question', () => {
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

        expect(question.number).toBe(expectedNumber);
        expect(question.type).toBe(expectedType);
        expect(question.question).toBe(expectedQuestion);
        expect(question.answer).toBe(expectedAnswer);
        expect(question.isWhatLinks).toBe(expectedIsWhatLinks);
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

        expect(question.number).toBe(expectedNumber);
        expect(question.type).toBe(expectedType);
        expect(question.question).toBe(expectedQuestion);
        expect(question.answer).toBe(expectedAnswer);
        expect(question.isWhatLinks).toBe(expectedIsWhatLinks);
    });
});
