using NUnit.Framework;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.UnitTests.Utils;

public class Utf8ContentTypeProviderTests
{
    [TestCase(".css", "text/css; charset=utf-8")]
    [TestCase(".js", "text/javascript; charset=utf-8")]
    [TestCase(".html", "text/html; charset=utf-8")]
    [TestCase(".svg", "image/svg+xml; charset=utf-8")]
    public void WhenContentTypeProviderIsConstructed_ThenContentTypeMappingsHaveCharsetUtf8(
        string fileExtension,
        string expectedContentType)
    {
        // When
        var provider = new Utf8ContentTypeProvider();

        // Then
        Assert.That(provider.Mappings[fileExtension], Is.EqualTo(expectedContentType));
    }
}
