import { suite, test } from 'mocha';
import assert from 'assert';
import TimeSpan from '../../SaturdayQuizWeb/wwwroot/script/TimeSpan.js';

suite('TimeSpan', () => {
    test('GIVEN TimeSpan with all parts WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = new TimeSpan(12345678);
        assert.strictEqual(timeSpan.toString(), '12345s 678ms');
    });

    test('GIVEN TimeSpan with 1 millisecond WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = new TimeSpan(1);
        assert.strictEqual(timeSpan.toString(), '1ms');
    });

    test('GIVEN TimeSpan from 10 seconds WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = TimeSpan.fromSeconds(10);
        assert.strictEqual(timeSpan.toString(), '10s');
    });
});
