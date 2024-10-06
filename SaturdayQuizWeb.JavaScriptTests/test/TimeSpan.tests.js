import { suite, test } from 'mocha';
import assert from 'assert';
import TimeSpan from '../../SaturdayQuizWeb/wwwroot/script/TimeSpan.js';

suite('TimeSpan', () => {
    test('GIVEN TimeSpan with all parts WHEN converting to string THEN expected string is returned', () => {
        const daysMs = 2 * 24 * 60 * 60 * 1000;
        const hoursMs = 3 * 60 * 60 * 1000;
        const minutesMs = 4 * 60 * 1000;
        const secondsMs = 56 * 1000;
        const ms = 789;
        const timeSpan = new TimeSpan(daysMs + hoursMs + minutesMs + secondsMs + ms);
        assert.strictEqual(timeSpan.toString(), '2d 3h 4m 56s 789ms');
    });

    test('GIVEN TimeSpan from 1 day WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = TimeSpan.fromDays(1);
        assert.strictEqual(timeSpan.toString(), '1d');
    });

    test('GIVEN TimeSpan from 1 hour WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = TimeSpan.fromHours(1);
        assert.strictEqual(timeSpan.toString(), '1h');
    });

    test('GIVEN TimeSpan from 1 minute WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = TimeSpan.fromMinutes(1);
        assert.strictEqual(timeSpan.toString(), '1m');
    });

    test('GIVEN TimeSpan from 1 second WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = TimeSpan.fromSeconds(1);
        assert.strictEqual(timeSpan.toString(), '1s');
    });

    test('GIVEN TimeSpan with 1 millisecond WHEN converting to string THEN expected string is returned', () => {
        const timeSpan = new TimeSpan(1);
        assert.strictEqual(timeSpan.toString(), '1ms');
    });

    test('GIVEN TimeSpan and a lesser TimeSpan WHEN comparing with isLessThan THEN true is returned', () => {
        const timeSpan1 = new TimeSpan(1);
        const timeSpan2 = new TimeSpan(2);
        assert.ok(timeSpan1.isLessThan(timeSpan2));
    });

    test('GIVEN TimeSpan and a greater TimeSpan WHEN comparing with isLessThan THEN false is returned', () => {
        const timeSpan1 = new TimeSpan(2);
        const timeSpan2 = new TimeSpan(1);
        assert.ok(!timeSpan1.isLessThan(timeSpan2));
    });

    test('GIVEN two equal TimeSpans WHEN comparing with isLessThan THEN false is returned', () => {
        const timeSpan1 = new TimeSpan(1);
        const timeSpan2 = new TimeSpan(1);
        assert.ok(!timeSpan1.isLessThan(timeSpan2));
    });
});
