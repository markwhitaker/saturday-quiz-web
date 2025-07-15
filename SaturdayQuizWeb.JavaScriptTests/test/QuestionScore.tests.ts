import { suite, test } from 'mocha';
import assert from 'assert';
import QuestionScore from '../../SaturdayQuizWeb/wwwroot/script/src/QuestionScore.js';

suite('QuestionScore', () => {
    test('QuestionScore.NONE should be 0', () => {
        assert.strictEqual(QuestionScore.NONE, 0);
    });

    test('QuestionScore.HALF should be 0.5', () => {
        assert.strictEqual(QuestionScore.HALF, 0.5);
    });

    test('QuestionScore.FULL should be 1', () => {
        assert.strictEqual(QuestionScore.FULL, 1);
    });
});
