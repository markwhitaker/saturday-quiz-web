namespace SaturdayQuizWeb.Extensions;

public static class TimeSpanExtensions
{
    public static string ToCacheControlHeaderValue(this TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeSpan), "Value must not be negative");
        }

        var seconds = timeSpan.TotalSeconds > int.MaxValue
            ? int.MaxValue
            : (int)timeSpan.TotalSeconds;

        return seconds == 0 ? "no-cache" : $"max-age={seconds}";
    }
}
