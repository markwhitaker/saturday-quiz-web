import { describe, test, expect, mock } from 'bun:test';
import CalendarDate from '../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js';
import LocalStore from '../../SaturdayQuizWeb/wwwroot/script/LocalStore.js';

describe('LocalStore', () => {
    test('GIVEN quiz date is set WHEN quiz date is retrieved THEN quiz date is correct', () => {
        const mockLocalStorageWrapper = {
            getItem: mock(key => key === 'quiz-date' ? '2020-01-02' : undefined),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });
        expect(localStore.getQuizDate().toString()).toBe('Thu Jan 02 2020');
    });

    test('GIVEN quiz date is not set WHEN quiz date is retrieved THEN quiz date is undefined', () => {
        const mockLocalStorageWrapper = {
            getItem: mock(),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });
        expect(localStore.getQuizDate()).toBeUndefined();
    });

    test('GIVEN a quiz date WHEN quiz date is set THEN quiz date is stored', () => {
        const expectedStoredValue = 'Thu Jan 02 2020';
        const mockLocalStorageWrapper = {
            getItem: mock(),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setQuizDate(new CalendarDate(new Date('2020-01-02')));

        expect(mockLocalStorageWrapper.setItem).toHaveBeenCalledWith('quiz-date', expectedStoredValue);
    });

    test('GIVEN a previous quiz date is already stored WHEN quiz date is set THEN scores are cleared AND quiz date is stored', () => {
        const expectedStoredValue = 'Thu Jan 02 2020';

        const mockLocalStorageWrapper = {
            getItem: mock(key => key === 'quiz-date' ? '2020-01-01' : undefined),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setQuizDate(new CalendarDate(new Date('2020-01-02')));

        expect(mockLocalStorageWrapper.setItem).toHaveBeenCalledWith('quiz-date', expectedStoredValue);
        expect(mockLocalStorageWrapper.removeItem).toHaveBeenCalledWith('scores');
    });

    test('GIVEN quiz json is stored WHEN quiz is retrieved THEN expected quiz is returned', () => {
        const storedQuizJson = '{"quiz":"json"}';
        const expectedQuiz = { 'quiz': 'json' };
        const mockLocalStorageWrapper = {
            getItem: mock(key => key === 'quiz-json' ? storedQuizJson : undefined),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        const actualQuiz = localStore.getQuiz();

        expect(actualQuiz).toEqual(expectedQuiz);
    });

    test('GIVEN in-memory quiz WHEN quiz is set THEN expected quiz json is stored', () => {
        const quiz = { 'quiz': 'json' };
        const expectedQuizJson = '{"quiz":"json"}';
        const mockLocalStorageWrapper = {
            getItem: mock(),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setQuiz(quiz);

        expect(mockLocalStorageWrapper.setItem).toHaveBeenCalledWith('quiz-json', expectedQuizJson);
    });

    test('GIVEN local store WHEN quiz is cleared THEN quiz json is removed', () => {
        const expectedRemovedKey = 'quiz-json';
        const mockLocalStorageWrapper = {
            getItem: mock(),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.clearQuiz();

        expect(mockLocalStorageWrapper.removeItem).toHaveBeenCalledWith(expectedRemovedKey);
    });

    test('GIVEN scores are stored WHEN scores are retrieved THEN expected scores are returned', () => {
        const expectedScores = 'expected-scores';
        const mockLocalStorageWrapper = {
            getItem: mock(key => key === 'scores' ? expectedScores : undefined),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        const actualScores = localStore.getScores();

        expect(actualScores).toBe(expectedScores);
    });

    test('GIVEN scores WHEN scores are set THEN expected scores are stored', () => {
        const expectedStoredScores = 'expected-scores';
        const mockLocalStorageWrapper = {
            getItem: mock(),
            setItem: mock(),
            removeItem: mock()
        };
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setScores(expectedStoredScores);

        expect(mockLocalStorageWrapper.setItem).toHaveBeenCalledWith('scores', expectedStoredScores);
    });
});
