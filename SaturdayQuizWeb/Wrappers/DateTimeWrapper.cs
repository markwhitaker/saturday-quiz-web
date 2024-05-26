namespace SaturdayQuizWeb.Wrappers;

public class DateTimeWrapper : IDateTimeWrapper
{
    public DateTime UtcNow => DateTime.UtcNow;
}
