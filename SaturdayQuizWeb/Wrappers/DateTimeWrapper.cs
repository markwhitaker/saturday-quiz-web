namespace SaturdayQuizWeb.Wrappers;

public sealed class DateTimeWrapper : IDateTimeWrapper
{
    public DateTime UtcNow => DateTime.UtcNow;
}
