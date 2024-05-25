using RegexToolbox;
using SaturdayQuizWeb.Extensions;

namespace SaturdayQuizWeb.UnitTests.Extensions;

[TestFixture]
[TestOf(typeof(RegexBuilderExtensions))]
public class RegexBuilderExtensionsTests
{
    [Test]
    public void GivenRegexBuilder_WhenPossibleHtmlWhitespaceAdded_ThenExpectedRegexIsBuilt()
    {
        // Given
        const string expectedRegexString = @"(?: |\\t|\\n|\\r|&nbsp;)*";
        var regexBuilder = new RegexBuilder();

        // When
        var actualRegexString = regexBuilder
            .PossibleHtmlWhitespace()
            .BuildRegex()
            .ToString();

        // Then
        Assert.That(actualRegexString, Is.EqualTo(expectedRegexString));
    }

    [Test]
    public void GivenRegexBuilder_WhenHtmlOpenTagAdded_ThenExpectedRegexIsBuilt()
    {
        // Given
        const string tagName = "a";
        const string expectedRegexString = @"<\s*a\b[^>]*>";
        var regexBuilder = new RegexBuilder();

        // When
        var actualRegexString = regexBuilder
            .HtmlOpenTag(tagName)
            .BuildRegex()
            .ToString();

        // Then
        Assert.That(actualRegexString, Is.EqualTo(expectedRegexString));
    }

    [Test]
    public void GivenRegexBuilder_WhenHtmlCloseTagAdded_ThenExpectedRegexIsBuilt()
    {
        // Given
        const string tagName = "a";
        const string expectedRegexString = @"</\s*a\b[^>]*>";
        var regexBuilder = new RegexBuilder();

        // When
        var actualRegexString = regexBuilder
            .HtmlCloseTag(tagName)
            .BuildRegex()
            .ToString();

        // Then
        Assert.That(actualRegexString, Is.EqualTo(expectedRegexString));
    }
}
