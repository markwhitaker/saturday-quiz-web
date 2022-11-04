using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.UnitTests;

[TestFixture]
public class Utf8ContentTypeProviderTests
{
    private readonly IContentTypeProvider _provider = new Utf8ContentTypeProvider();

    [TestCase(".css", "text/css; charset=utf-8")]
    [TestCase(".CSS", "text/css; charset=utf-8")]
    [TestCase(".html", "text/html; charset=utf-8")]
    [TestCase(".HTML", "text/html; charset=utf-8")]
    [TestCase(".js", "text/javascript; charset=utf-8")]
    [TestCase(".JS", "text/javascript; charset=utf-8")]
    [TestCase(".svg", "image/svg+xml; charset=utf-8")]
    [TestCase(".SVG", "image/svg+xml; charset=utf-8")]
    public void GivenCustomisedFileExtension_WhenMapped_ThenContentTypeHasCharsetUtf8(
        string fileExtension,
        string expectedContentType)
    {
        // Given (see TestCase)

        // When
        var found = _provider.TryGetContentType(fileExtension, out var actualContentType);

        // Then
        Assert.That(found, Is.True);
        Assert.That(actualContentType, Is.EqualTo(expectedContentType));
    }

    [TestCase(".jpeg", "image/jpeg")]
    [TestCase(".png", "image/png")]
    [TestCase(".woff2", "font/woff2")]
    public void GivenNonCustomisedFileExtension_WhenMapped_ThenContentTypeDoesNotHaveCharsetUtf8(
        string fileExtension,
        string expectedContentType)
    {
        // Given (see TestCase)

        // When
        var found = _provider.TryGetContentType(fileExtension, out var actualContentType);

        // Then
        Assert.That(found, Is.True);
        Assert.That(actualContentType, Is.EqualTo(expectedContentType));
    }
}
