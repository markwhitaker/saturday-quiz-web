import { suite, test } from 'mocha';
import assert from 'assert';
import Quiz from '../../SaturdayQuizWeb/wwwroot/script/src/Quiz.js';

suite('Quiz', () => {
    test('GIVEN raw quiz object WHEN quiz is constructed THEN expected quiz is returned', () => {
        const rawQuizObject = {
            date: '2020-01-02 12:34:56',
            questions: [
                { number: 1, type: 'NORMAL', question: 'question-1', answer: 'answer-1' },
                { number: 2, type: 'WHAT_LINKS', question: 'question-2', answer: 'answer-2' }
            ]
        };

        const quiz = new Quiz(rawQuizObject);

        assert.strictEqual(quiz.date.toString(), 'Thu Jan 02 2020');
        assert.strictEqual(quiz.questions.length, 2);
        assert.strictEqual(quiz.questions[0].number, 1);
        assert.strictEqual(quiz.questions[0].type, 'NORMAL');
        assert.strictEqual(quiz.questions[0].question, 'question-1');
        assert.strictEqual(quiz.questions[0].answer, 'answer-1');
        assert.strictEqual(quiz.questions[0].isWhatLinks, false);
        assert.strictEqual(quiz.questions[1].number, 2);
        assert.strictEqual(quiz.questions[1].type, 'WHAT_LINKS');
        assert.strictEqual(quiz.questions[1].question, 'question-2');
        assert.strictEqual(quiz.questions[1].answer, 'answer-2');
        assert.strictEqual(quiz.questions[1].isWhatLinks, true);
    });
});
