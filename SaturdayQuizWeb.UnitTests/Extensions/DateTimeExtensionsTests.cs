using SaturdayQuizWeb.Extensions;

namespace SaturdayQuizWeb.UnitTests.Extensions;

[TestFixture]
[TestOf(typeof(DateTimeExtensions))]
public class DateTimeExtensionsTests
{

    [TestCase(DateTimeKind.Local)]
    [TestCase(DateTimeKind.Utc)]
    [TestCase(DateTimeKind.Unspecified)]
    public void GivenAnyDateTime_WhenToDateUtc_ThenUtcDateIsReturned(DateTimeKind dateTimeKind)
    {
        // Given
        var localDateTime = new DateTime(2022, 8, 1, 12, 34, 56, dateTimeKind);
        var expectedUtcDate = new DateTime(2022, 8, 1, 0, 0, 0, DateTimeKind.Utc);

        // When
        var actualUtcDate = localDateTime.ToDateUtc();

        // Then
        Assert.That(actualUtcDate, Is.EqualTo(expectedUtcDate));
    }
}
