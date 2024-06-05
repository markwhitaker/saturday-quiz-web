import { suite, test } from 'mocha';
import assert from 'assert';
import CalendarDate from '../../SaturdayQuizWeb/wwwroot/script/CalendarDate.js';

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

    test('GIVEN two different dates WHEN compared THEN result is correct', () => {
        const earlierCalendarDate = new CalendarDate(new Date('2020-01-02'));
        const laterCalendarDate = new CalendarDate(new Date('2020-01-03'));
        assert.ok(laterCalendarDate.isGreaterThan(earlierCalendarDate));
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

    test('GIVEN date WHEN converting to locale string THEN result is correct', () => {
        const date = new Date('2020-01-02');
        const calendarDate = new CalendarDate(date);
        const calendarDateLocaleString = calendarDate.toLocaleString('en-GB');
        assert.strictEqual(calendarDateLocaleString, '02/01/2020');
    });

    test('GIVEN date WHEN converting to locale string with options THEN result is correct', () => {
        const date = new Date('2020-01-02');
        const calendarDate = new CalendarDate(date);
        const calendarDateLocaleString = calendarDate.toLocaleString('en-GB', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });
        assert.strictEqual(calendarDateLocaleString, 'Thursday 2 January 2020');
    });
});
