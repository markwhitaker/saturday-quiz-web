using SaturdayQuizWeb.Extensions;

namespace SaturdayQuizWeb.UnitTests.Extensions;

[TestFixture]
public class TimeSpanExtensionsTests
{
    [TestCase(0, "no-cache")]
    [TestCase(1, "max-age=1")]
    [TestCase(int.MaxValue, "max-age=2147483647")]
    public void GivenTimeSpanInSeconds_WhenConvertedToCacheControlHeaderValue_ThenExpectedValueIsReturned(int seconds, string expectedHeaderValue)
    {
        // Given
        var timeSpan = TimeSpan.FromSeconds(seconds);

        // When
        var actualHeaderValue = timeSpan.ToCacheControlHeaderValue();

        // Then
        Assert.That(actualHeaderValue, Is.EqualTo(expectedHeaderValue));
    }

    [TestCase(0, "no-cache")]
    [TestCase(1, "max-age=60")]
    [TestCase(int.MaxValue, "max-age=2147483647")]
    public void GivenTimeSpanInMinutes_WhenConvertedToCacheControlHeaderValue_ThenExpectedValueIsReturned(int seconds, string expectedHeaderValue)
    {
        // Given
        var timeSpan = TimeSpan.FromMinutes(seconds);

        // When
        var actualHeaderValue = timeSpan.ToCacheControlHeaderValue();

        // Then
        Assert.That(actualHeaderValue, Is.EqualTo(expectedHeaderValue));
    }

    [Test]
    public void GivenNegativeTimeSpan_WhenConvertedToCacheControlHeaderValue_ThenExceptionIsThrown()
    {
        // Given
        var timeSpan = TimeSpan.FromSeconds(-1);

        // When/Then
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => timeSpan.ToCacheControlHeaderValue());

        Assert.That(exception!.Message, Is.EqualTo("Value must not be negative (Parameter 'timeSpan')"));
    }
}
