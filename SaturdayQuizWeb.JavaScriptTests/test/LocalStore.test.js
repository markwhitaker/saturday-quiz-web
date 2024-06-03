import { suite, test } from 'mocha';
import assert from 'assert';
import CalendarDate from "../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js";
import LocalStore from '../../SaturdayQuizWeb/wwwroot/script/LocalStore.js';
import LocalStoreWrapperMockBuilder from "./mocks/LocalStoreWrapperMockBuilder.js";

suite('LocalStore', function() {
    test('GIVEN localStorageWrapper is not an instance of LocalStorageWrapper WHEN LocalStore is constructed THEN error is thrown', () => {
        const notALocalStorageWrapper = {};
        assert.throws(() => new LocalStore(notALocalStorageWrapper), { name: 'Error', message: 'localStorageWrapper must be an instance of LocalStorageWrapper' });
    });

    test('GIVEN a mock local storage wrapper WHEN an instance of LocalStore is constructed THEN instance is OK', () => {
        const mockLocalStorageWrapper = new LocalStoreWrapperMockBuilder().build();
        const localStore = new LocalStore(mockLocalStorageWrapper);
        assert.ok(localStore);
    })

    test('GIVEN quiz date is set WHEN quiz date is retrieved THEN quiz date is correct', () => {
        const mockLocalStorageWrapper = new LocalStoreWrapperMockBuilder()
            .getItem(key => key === 'quiz-date' ? '2020-01-02' : undefined)
            .build();
        const localStore = new LocalStore(mockLocalStorageWrapper);
        assert.strictEqual(localStore.quizDate.toString(), "Thu Jan 02 2020");
    });

    test('GIVEN quiz date is not set WHEN quiz date is retrieved THEN quiz date is undefined', () => {
        const mockLocalStorageWrapper = new LocalStoreWrapperMockBuilder().build();
        const localStore = new LocalStore(mockLocalStorageWrapper);
        assert.strictEqual(localStore.quizDate, undefined);
    });

    test('GIVEN a quiz date WHEN quiz date is set THEN quiz date is stored', () => {
        const expectedStoredValue = 'Thu Jan 02 2020';
        let actualStoredValue = '';
        const mockLocalStorageWrapper = new LocalStoreWrapperMockBuilder()
            .setItem((key, value) => key === 'quiz-date' ? actualStoredValue = value : undefined)
            .build();
        const localStore = new LocalStore(mockLocalStorageWrapper);

        localStore.quizDate = new CalendarDate(new Date('2020-01-02'));

        assert.strictEqual(actualStoredValue, expectedStoredValue);
    });

    test('GIVEN a previous quiz date is already stored WHEN quiz date is set THEN scores are cleared AND quiz date is stored', () => {
        const expectedStoredValue = 'Thu Jan 02 2020';
        let actualStoredValue = '';
        let scoresCleared = false;

        const mockLocalStorageWrapper = new LocalStoreWrapperMockBuilder()
            .getItem(key => key === 'quiz-date' ? '2020-01-01' : undefined)
            .setItem((key, value) => key === 'quiz-date' ? actualStoredValue = value : undefined)
            .removeItem(key => scoresCleared = key === 'scores')
            .build();
        const localStore = new LocalStore(mockLocalStorageWrapper);

        localStore.quizDate = new CalendarDate(new Date('2020-01-02'));

        assert.strictEqual(actualStoredValue, expectedStoredValue);
        assert.strictEqual(scoresCleared, true);
    });

});
