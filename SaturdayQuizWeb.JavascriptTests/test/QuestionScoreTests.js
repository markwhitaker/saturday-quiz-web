import assert from 'assert';
import QuestionScore from '../../SaturdayQuizWeb/wwwroot/script/QuestionScore.js';

it('QuestionScore.NONE should be 0', () => {
    assert.equal(QuestionScore.NONE, 0);
});

it('QuestionScore.HALF should be 0.5', () => {
    assert.equal(QuestionScore.HALF, 0.5);
});

it('QuestionScore.FULL should be 1', () => {
    assert.equal(QuestionScore.FULL, 1);
});
