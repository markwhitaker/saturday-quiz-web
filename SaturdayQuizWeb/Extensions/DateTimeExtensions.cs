namespace SaturdayQuizWeb.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToDateUtc(this DateTime dateTime) =>
        new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
}
