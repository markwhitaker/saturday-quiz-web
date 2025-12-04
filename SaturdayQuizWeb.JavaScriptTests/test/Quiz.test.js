import { describe, test, expect } from 'bun:test';
import Quiz from '../../SaturdayQuizWeb/wwwroot/script/Quiz.js';

describe('Quiz', () => {
    test('GIVEN raw quiz object WHEN quiz is constructed THEN expected quiz is returned', () => {
        const rawQuizObject = {
            date: '2020-01-02 12:34:56',
            questions: [
                { number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' },
                { number: 2, type: 'WHAT_LINKS', question: 'question-2', answer: 'answer-2' }
            ]
        };

        const quiz = new Quiz(rawQuizObject);

        expect(quiz.getDate().toString()).toBe('Thu Jan 02 2020');
        expect(quiz.getQuestions().length).toBe(2);
        expect(quiz.getQuestions()[0].getNumber()).toBe(1);
        expect(quiz.getQuestions()[0].getType()).toBe('NORMAL');
        expect(quiz.getQuestions()[0].getQuestion()).toBe('question-1');
        expect(quiz.getQuestions()[0].getAnswer()).toBe('answer-1');
        expect(quiz.getQuestions()[0].isWhatLinks()).toBe(false);
        expect(quiz.getQuestions()[1].getNumber()).toBe(2);
        expect(quiz.getQuestions()[1].getType()).toBe('WHAT_LINKS');
        expect(quiz.getQuestions()[1].getQuestion()).toBe('question-2');
        expect(quiz.getQuestions()[1].getAnswer()).toBe('answer-2');
        expect(quiz.getQuestions()[1].isWhatLinks()).toBe(true);
    });
});
