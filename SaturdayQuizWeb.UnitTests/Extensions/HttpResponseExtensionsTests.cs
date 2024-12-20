using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SaturdayQuizWeb.Extensions;

namespace SaturdayQuizWeb.UnitTests.Extensions;

[TestFixture]
[TestOf(typeof(HttpResponseExtensions))]
public class HttpResponseExtensionsTests
{
    [Test]
    public void GivenHttpResponse_WhenAddCustomHeadersIsCalled_ThenExpectedHeadersAreAdded()
    {
        // Given
        var cacheDuration = TimeSpan.FromSeconds(10);
        const string expectedCacheControlHeaderValue = "max-age=10";
        const string expectedXContentTypeOptionsHeaderValue = "nosniff";

        var httpResponse = new TestHttpResponse();

        // When
        httpResponse.AddCustomHeaders(cacheDuration);

        // Then
        Assert.That(httpResponse.Headers, Has.Exactly(2).Items);

        Assert.That(httpResponse.Headers, Contains.Key(HeaderNames.CacheControl));
        Assert.That(httpResponse.Headers.CacheControl.ToString(), Is.EqualTo(expectedCacheControlHeaderValue));

        Assert.That(httpResponse.Headers, Contains.Key(HeaderNames.XContentTypeOptions));
        Assert.That(httpResponse.Headers.XContentTypeOptions.ToString(), Is.EqualTo(expectedXContentTypeOptionsHeaderValue));
    }
}

file class TestHttpResponse : HttpResponse
{
    public override void OnStarting(Func<object, Task> callback, object state)
    {
    }

    public override void OnCompleted(Func<object, Task> callback, object state)
    {
    }

    public override void Redirect(string location, bool permanent)
    {
    }

    public override HttpContext HttpContext => default!;
    public override int StatusCode { get; set; }
    public override IHeaderDictionary Headers { get; } = new HeaderDictionary();
    public override Stream Body { get; set; } = default!;
    public override long? ContentLength { get; set; }
    public override string? ContentType { get; set; }
    public override IResponseCookies Cookies => default!;
    public override bool HasStarted => default;
}
