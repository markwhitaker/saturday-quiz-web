using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.UnitTests.Wrappers;

[TestFixture]
[TestOf(typeof(DateTimeWrapper))]
public class DateTimeWrapperTests
{
    private readonly IDateTimeWrapper _dateTimeWrapper = new DateTimeWrapper();

    [Test]
    public void GivenDateTimeWrapper_WhenUtcNow_ThenCloseToDateTimeUtcNow()
    {
        // When
        var actualDateTimeUtc = _dateTimeWrapper.UtcNow;

        // Then
        Assert.That(actualDateTimeUtc, Is.EqualTo(DateTime.UtcNow).Within(10).Seconds);
    }
}
