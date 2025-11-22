namespace SaturdayQuizWeb.Wrappers;

public interface IDateTimeWrapper
{
    DateTime UtcNow { get; }
}

public sealed class DateTimeWrapper : IDateTimeWrapper
{
    public DateTime UtcNow => DateTime.UtcNow;
}