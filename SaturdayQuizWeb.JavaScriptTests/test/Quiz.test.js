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

        expect(quiz.date.toString()).toBe('Thu Jan 02 2020');
        expect(quiz.questions.length).toBe(2);
        expect(quiz.questions[0].number).toBe(1);
        expect(quiz.questions[0].type).toBe('NORMAL');
        expect(quiz.questions[0].question).toBe('question-1');
        expect(quiz.questions[0].answer).toBe('answer-1');
        expect(quiz.questions[0].isWhatLinks).toBe(false);
        expect(quiz.questions[1].number).toBe(2);
        expect(quiz.questions[1].type).toBe('WHAT_LINKS');
        expect(quiz.questions[1].question).toBe('question-2');
        expect(quiz.questions[1].answer).toBe('answer-2');
        expect(quiz.questions[1].isWhatLinks).toBe(true);
    });
});
