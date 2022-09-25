using System;

namespace SaturdayQuizWeb.Extensions;

public static class TimeSpanExtensions
{
    public static string ToCacheControlHeaderValue(this TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeSpan), "Value must not be negative");
        }

        var seconds = (int)timeSpan.TotalSeconds;
        if (seconds < 0)
        {
            seconds = int.MaxValue;
        }

        return seconds == 0 ? "no-cache" : $"max-age={seconds}";
    }
}
