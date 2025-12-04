import { describe, test, expect, mock } from 'bun:test';
import QuizCache from '../../SaturdayQuizWeb/wwwroot/script/QuizCache.js';
import CalendarDate from "../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js";

describe('QuizCache', () => {
    test('GIVEN quiz is not cached WHEN get cached quiz THEN stored quiz is cleared and undefined is returned', () => {
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(6);

        const mockLocalStore = {
            clearQuiz: mock(),
            getQuizDate: mock(() => cachedQuizDate),
            getQuiz: mock(() => undefined),
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            setQuizDate: mock(),
            setQuiz: mock(),
            getScores: mock(),
            setScores: mock()
        };
        const quizCache = new QuizCache({
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        expect(actualCachedQuiz).toBeUndefined();
        expect(mockLocalStore.clearQuiz).toHaveBeenCalled();
    });

    test('GIVEN quiz is cached and 7 days old WHEN get cached quiz THEN stored quiz is cleared and undefined is returned', () => {
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(7);
        const cachedQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };

        const mockLocalStore = {
            clearQuiz: mock(),
            getQuizDate: mock(() => cachedQuizDate),
            getQuiz: mock(() => cachedQuiz),
            getQuizCacheHitTimestamp: mock(),
            setQuizCacheHitTimestamp: mock(),
            setQuizDate: mock(),
            setQuiz: mock(),
            getScores: mock(),
            setScores: mock()
        };
        const quizCache = new QuizCache({
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        expect(actualCachedQuiz).toBeUndefined();
        expect(mockLocalStore.clearQuiz).toHaveBeenCalled();
    });

    test(`GIVEN quiz is cached and less than 7 days old and cache was hit within last ${QuizCache.skipCacheIfReloadedWithin} WHEN get cached quiz THEN stored quiz is cleared and undefined is returned`, () => {
        const cacheHitTimestamp = 0;
        const nowTimestamp = QuizCache.skipCacheIfReloadedWithin.getMilliseconds();
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(6);
        const cachedQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };

        const mockLocalStore = {
            clearQuiz: mock(),
            getQuizCacheHitTimestamp: mock(() => cacheHitTimestamp),
            getQuizDate: mock(() => cachedQuizDate),
            getQuiz: mock(() => cachedQuiz),
            setQuizCacheHitTimestamp: mock(),
            setQuizDate: mock(),
            setQuiz: mock(),
            getScores: mock(),
            setScores: mock()
        };
        const mockDateWrapper = {
            getNow: mock(() => nowTimestamp)
        };
        const quizCache = new QuizCache({
            dateWrapper: mockDateWrapper,
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        expect(actualCachedQuiz).toBeUndefined();
        expect(mockLocalStore.clearQuiz).toHaveBeenCalled();
    });

    test(`GIVEN quiz is cached and less than 7 days old and cache was not hit within last ${QuizCache.skipCacheIfReloadedWithin} WHEN get cached quiz THEN stored quiz is not cleared and cached quiz is returned`, () => {
        const cacheHitTimestamp = 0;
        const nowTimestamp = QuizCache.skipCacheIfReloadedWithin.getMilliseconds() + 1;
        const cachedQuizDate = new CalendarDate(new Date()).subtractDays(6);
        const expectedCachedQuiz = {
            'date': '2020-01-02',
            'questions': [{ 'number': 1, 'type': 'NORMAL', 'question': 'question', 'answer': 'answer' }]
        };

        const mockLocalStore = {
            clearQuiz: mock(),
            getQuizCacheHitTimestamp: mock(() => cacheHitTimestamp),
            getQuizDate: mock(() => cachedQuizDate),
            getQuiz: mock(() => expectedCachedQuiz),
            setQuizCacheHitTimestamp: mock(),
            setQuizDate: mock(),
            setQuiz: mock(),
            getScores: mock(),
            setScores: mock()
        };
        const mockDateWrapper = {
            getNow: mock(() => nowTimestamp)
        };
        const quizCache = new QuizCache({
            dateWrapper: mockDateWrapper,
            localStore: mockLocalStore
        });

        const actualCachedQuiz = quizCache.getCachedQuiz();

        expect(actualCachedQuiz).toBe(expectedCachedQuiz);
        expect(mockLocalStore.clearQuiz).not.toHaveBeenCalled();
    });
});
