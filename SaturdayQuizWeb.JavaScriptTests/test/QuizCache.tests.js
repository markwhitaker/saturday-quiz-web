import { suite, test } from 'mocha';
import assert from 'assert';
import QuizCache from '../../SaturdayQuizWeb/wwwroot/script/QuizCache.js';
import CalendarDate from "../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js";
import LocalStoreMockBuilder from "./mocks/MockLocalStoreBuilder.js";
import MockDateWrapperBuilder from "./mocks/MockDateWrapperBuilder.js";

suite('QuizCache', () => {
    test('GIVEN quiz is not cached WHEN get cached quiz THEN stored quiz is cleared and undefined is returned', () => {
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(6);

        let actualQuizCleared = false;

        const mockLocalStore = new LocalStoreMockBuilder()
            .clearQuiz(() => actualQuizCleared = true)
            .getQuizDate(() => cachedQuizDate)
            .getQuiz(() => undefined)
            .build();
        const quizCache = new QuizCache({
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        assert.strictEqual(actualCachedQuiz, undefined);
        assert.strictEqual(actualQuizCleared, true);
    });

    test('GIVEN quiz is cached and 7 days old WHEN get cached quiz THEN stored quiz is cleared and undefined is returned', () => {
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(7);
        const cachedQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };

        let actualQuizCleared = false;

        const mockLocalStore = new LocalStoreMockBuilder()
            .clearQuiz(() => actualQuizCleared = true)
            .getQuizDate(() => cachedQuizDate)
            .getQuiz(() => cachedQuiz)
            .build();
        const quizCache = new QuizCache({
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        assert.strictEqual(actualCachedQuiz, undefined);
        assert.strictEqual(actualQuizCleared, true);
    });

    test(`GIVEN quiz is cached and less than 7 days old and cache was hit within last ${QuizCache.skipCacheIfReloadedWithin} WHEN get cached quiz THEN stored quiz is cleared and undefined is returned`, () => {
        const cacheHitTimestamp = 0;
        const nowTimestamp = QuizCache.skipCacheIfReloadedWithin.milliseconds;
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(6);
        const cachedQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };

        let actualQuizCleared = false;

        const mockLocalStore = new LocalStoreMockBuilder()
            .clearQuiz(() => actualQuizCleared = true)
            .getQuizCacheHitTimestamp(() => cacheHitTimestamp)
            .getQuizDate(() => cachedQuizDate)
            .getQuiz(() => cachedQuiz)
            .build();
        const mockDateWrapper = new MockDateWrapperBuilder()
            .getNow(() => nowTimestamp)
            .build();
        const quizCache = new QuizCache({
            dateWrapper: mockDateWrapper,
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        assert.strictEqual(actualCachedQuiz, undefined);
        assert.strictEqual(actualQuizCleared, true);
    });

    test(`GIVEN quiz is cached and less than 7 days old and cache was not hit within last ${QuizCache.skipCacheIfReloadedWithin} WHEN get cached quiz THEN stored quiz is not cleared and cached quiz is returned`, () => {
        const cacheHitTimestamp = 0;
        const nowTimestamp = QuizCache.skipCacheIfReloadedWithin.milliseconds + 1;
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(6);
        const expectedCachedQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };

        let actualQuizCleared = false;

        const mockLocalStore = new LocalStoreMockBuilder()
            .clearQuiz(() => actualQuizCleared = true)
            .getQuizCacheHitTimestamp(() => cacheHitTimestamp)
            .getQuizDate(() => cachedQuizDate)
            .getQuiz(() => expectedCachedQuiz)
            .build();
        const mockDateWrapper = new MockDateWrapperBuilder()
            .getNow(() => nowTimestamp)
            .build();
        const quizCache = new QuizCache({
            dateWrapper: mockDateWrapper,
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        assert.deepStrictEqual(actualCachedQuiz, expectedCachedQuiz);
        assert.strictEqual(actualQuizCleared, false);
    });
});
