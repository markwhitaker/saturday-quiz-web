import {expect, test} from '@jest/globals';
import QuestionScore from "../../SaturdayQuizWeb/wwwroot/script/QuestionScore.js";

test("QuestionScore.NONE should be 0", () => {
    expect(QuestionScore.NONE).toBe(0);
});

test("QuestionScore.HALF should be 0.5", () => {
    expect(QuestionScore.HALF).toBe(0.5);
});

test("QuestionScore.FULL should be 1", () => {
    expect(QuestionScore.FULL).toBe(1);
});
