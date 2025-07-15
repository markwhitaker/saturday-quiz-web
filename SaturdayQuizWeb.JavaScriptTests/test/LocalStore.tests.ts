import { suite, test } from 'mocha';
import assert from 'assert';
import CalendarDate from '../../SaturdayQuizWeb/wwwroot/script/src/CalendarDate.js';
import LocalStore from '../../SaturdayQuizWeb/wwwroot/script/src/LocalStore.js';
import MockLocalStorageWrapperBuilder from './mocks/MockLocalStorageWrapperBuilder.js';

suite('LocalStore', () => {
    test('GIVEN quiz date is set WHEN quiz date is retrieved THEN quiz date is correct', () => {
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .getItem(key => key === 'quiz-date' ? '2020-01-02' : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });
        assert.strictEqual(localStore.quizDate.toString(), 'Thu Jan 02 2020');
    });

    test('GIVEN quiz date is not set WHEN quiz date is retrieved THEN quiz date is undefined', () => {
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder().build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });
        assert.strictEqual(localStore.quizDate, undefined);
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

        localStore.quizDate = new CalendarDate(new Date('2020-01-02'));

        assert.strictEqual(actualStoredValue, expectedStoredValue);
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

        localStore.quizDate = new CalendarDate(new Date('2020-01-02'));

        assert.strictEqual(actualStoredValue, expectedStoredValue);
        assert.strictEqual(scoresCleared, true);
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

        const actualQuiz = localStore.quiz;

        assert.deepStrictEqual(actualQuiz, expectedQuiz);
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

        localStore.quiz = quiz;

        assert.strictEqual(actualQuizJson, expectedQuizJson);
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

        assert.strictEqual(actualRemovedKey, expectedRemovedKey);
    });

    test('GIVEN scores are stored WHEN scores are retrieved THEN expected scores are returned', () => {
        const expectedScores = 'expected-scores';
        const mockLocalStorageWrapper = new MockLocalStorageWrapperBuilder()
            .getItem(key => key === 'scores' ? expectedScores : undefined)
            .build();
        const localStore = new LocalStore({
            localStorageWrapper: mockLocalStorageWrapper
        });

        const actualScores = localStore.scores;

        assert.strictEqual(actualScores, expectedScores);
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

        localStore.scores = expectedStoredScores;

        assert.strictEqual(actualStoredScores, expectedStoredScores);
    });
});
