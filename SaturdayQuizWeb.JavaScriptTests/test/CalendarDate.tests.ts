import { suite, test } from 'mocha';
import assert from 'assert';
import CalendarDate from '../../SaturdayQuizWeb/wwwroot/script/src/CalendarDate.js';
import TimeSpan from "../../SaturdayQuizWeb/wwwroot/script/src/TimeSpan.js";

suite('CalendarDate', () => {
    test ('GIVEN date with time WHEN CalendarDate is constructed THEN time is removed', () => {
        const date = new Date('2020-01-02T12:34:56');
        const calendarDate = new CalendarDate(date);
        assert.strictEqual(calendarDate.toString(), 'Thu Jan 02 2020');
    });

    test ('GIVEN two dates with same date and different times WHEN converted to CalendarDates THEN CalendarDates are equal', () => {
        const date1 = new Date('2020-01-02T12:34:56');
        const date2 = new Date('2020-01-02T23:45:01');
        const calendarDate1 = new CalendarDate(date1);
        const calendarDate2 = new CalendarDate(date2);
        assert.ok(calendarDate1.equals(calendarDate2));
    });

    test('GIVEN earlier date WHEN diffed with later date THEN result is correct', () => {
        const earlierCalendarDate = new CalendarDate(new Date('2020-01-01'));
        const laterCalendarDate = new CalendarDate(new Date('2020-01-02'));
        const expectedDiff = TimeSpan.fromDays(1);
        const actualDiff = earlierCalendarDate.diff(laterCalendarDate);
        assert.ok(expectedDiff.equals(actualDiff));
    });

    test('GIVEN later date WHEN diffed with earlier date THEN result is correct', () => {
        const earlierCalendarDate = new CalendarDate(new Date('2020-01-01'));
        const laterCalendarDate = new CalendarDate(new Date('2020-01-02'));
        const expectedDiff = TimeSpan.fromDays(1);
        const actualDiff = laterCalendarDate.diff(earlierCalendarDate);
        assert.ok(expectedDiff.equals(actualDiff));
    });

    test('GIVEN date WHEN diffed with equal date THEN result is correct', () => {
        const calendarDate1 = new CalendarDate(new Date('2020-01-01'));
        const calendarDate2 = new CalendarDate(new Date('2020-01-01'));
        const expectedDiff = new TimeSpan(0);
        const actualDiff = calendarDate1.diff(calendarDate2);
        assert.ok(expectedDiff.equals(actualDiff));
    });

    test('GIVEN date WHEN subtracting days THEN result is correct', () => {
        const date = new Date('2020-01-02');
        const calendarDate = new CalendarDate(date);
        const newCalendarDate = calendarDate.subtractDays(1);
        assert.strictEqual(newCalendarDate.toString(), 'Wed Jan 01 2020');
    });

    test('GIVEN date WHEN converting to string THEN result is correct', () => {
        const date = new Date('2020-01-02');
        const calendarDate = new CalendarDate(date);
        const calendarDateString = calendarDate.toString();
        assert.strictEqual(calendarDateString, 'Thu Jan 02 2020');
    });

    test('GIVEN date WHEN converting to display string THEN result is correct', () => {
        const date = new Date('2020-01-02');
        const calendarDate = new CalendarDate(date);
        const calendarDateLocaleString = calendarDate.toDisplayString();
        assert.strictEqual(calendarDateLocaleString, '2 January 2020');
    });
});
