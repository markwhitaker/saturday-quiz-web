using Mainwave.MimeTypes;

namespace SaturdayQuizWeb.IntegrationTests.Api;

[TestFixture]
public class SwaggerTests
{
    private HttpClient _httpClient = null!;

    [SetUp]
    public void SetUp() => _httpClient = new WebApplicationFactory<Program>().CreateClient();

    [Test]
    public void GivenValidSwaggerRequest_WhenRequestIsMade_ThenExpectedResponseIsReceived()
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "swagger/index.html"
        }.ToString();

        // When
        var response = _httpClient.GetAsync(requestUri).Result;

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Text.Html));
    }
}
