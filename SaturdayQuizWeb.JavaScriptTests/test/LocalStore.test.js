import { describe, test, expect } from 'bun:test';
import CalendarDate from '../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js';
import LocalStore from '../../SaturdayQuizWeb/wwwroot/script/LocalStore.js';
import MockLocalStorageWrapperBuilder from './mocks/MockLocalStorageWrapperBuilder.js';

describe('LocalStore', () => {
    test('GIVEN quiz date is set WHEN quiz date is retrieved THEN quiz date is correct', () => {
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .getItem(key => key === 'quiz-date' ? '2020-01-02' : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });
        expect(localStore.getQuizDate().toString()).toBe('Thu Jan 02 2020');
    });

    test('GIVEN quiz date is not set WHEN quiz date is retrieved THEN quiz date is undefined', () => {
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder().build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });
        expect(localStore.getQuizDate()).toBeUndefined();
    });

    test('GIVEN a quiz date WHEN quiz date is set THEN quiz date is stored', () => {
        const expectedStoredValue = 'Thu Jan 02 2020';
        let actualStoredValue = '';
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .setItem((key, value) => key === 'quiz-date' ? actualStoredValue = value : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setQuizDate(new CalendarDate(new Date('2020-01-02')));

        expect(actualStoredValue).toBe(expectedStoredValue);
    });

    test('GIVEN a previous quiz date is already stored WHEN quiz date is set THEN scores are cleared AND quiz date is stored', () => {
        const expectedStoredValue = 'Thu Jan 02 2020';
        let actualStoredValue = '';
        let scoresCleared = false;

        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .getItem(key => key === 'quiz-date' ? '2020-01-01' : undefined)
            .setItem((key, value) => key === 'quiz-date' ? actualStoredValue = value : undefined)
            .removeItem(key => scoresCleared = key === 'scores')
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setQuizDate(new CalendarDate(new Date('2020-01-02')));

        expect(actualStoredValue).toBe(expectedStoredValue);
        expect(scoresCleared).toBe(true);
    });

    test('GIVEN quiz json is stored WHEN quiz is retrieved THEN expected quiz is returned', () => {
        const storedQuizJson = '{"quiz":"json"}';
        const expectedQuiz = { 'quiz': 'json' };
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .getItem(key => key === 'quiz-json' ? storedQuizJson : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        const actualQuiz = localStore.getQuiz();

        expect(actualQuiz).toEqual(expectedQuiz);
    });

    test('GIVEN in-memory quiz WHEN quiz is set THEN expected quiz json is stored', () => {
        const quiz = { 'quiz': 'json' };
        const expectedQuizJson = '{"quiz":"json"}';
        let actualQuizJson = '';
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .setItem((key, value) => key === 'quiz-json' ? actualQuizJson = value : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setQuiz(quiz);

        expect(actualQuizJson).toBe(expectedQuizJson);
    });

    test('GIVEN local store WHEN quiz is cleared THEN quiz json is removed', () => {
        const expectedRemovedKey = 'quiz-json';
        let actualRemovedKey = '';
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .removeItem(key => actualRemovedKey = key)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.clearQuiz();

        expect(actualRemovedKey).toBe(expectedRemovedKey);
    });

    test('GIVEN scores are stored WHEN scores are retrieved THEN expected scores are returned', () => {
        const expectedScores = 'expected-scores';
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .getItem(key => key === 'scores' ? expectedScores : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        const actualScores = localStore.getScores();

        expect(actualScores).toBe(expectedScores);
    });

    test('GIVEN scores WHEN scores are set THEN expected scores are stored', () => {
        const expectedStoredScores = 'expected-scores';
        let actualStoredScores = '';
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .setItem((key, value) => key === 'scores' ? actualStoredScores = value : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        localStore.setScores(expectedStoredScores);

        expect(actualStoredScores).toBe(expectedStoredScores);
    });
});
