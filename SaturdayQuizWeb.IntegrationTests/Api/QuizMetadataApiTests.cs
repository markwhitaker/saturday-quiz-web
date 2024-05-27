using Mainwave.MimeTypes;
using Newtonsoft.Json.Linq;

namespace SaturdayQuizWeb.IntegrationTests.Api;

[TestFixture]
public class QuizMetadataApiTests
{
    private HttpClient _httpClient = null!;

    [SetUp]
    public void SetUp() => _httpClient = new WebApplicationFactory<Startup>().CreateClient();

    [Test]
    public async Task GivenValidQuizMetadataRequestWithoutCount_WhenRequestIsMade_ThenExpectedResponseIsReceived()
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz-metadata"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Application.Json));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Not.Null.Or.Empty);

        var quizMetadataArray = JArray.Parse(content);
        Assert.That(quizMetadataArray.Count, Is.EqualTo(10));

        var quizMetadata = quizMetadataArray[0];
        Assert.That(quizMetadata, Is.Not.Null.Or.Empty);
        Assert.That(quizMetadata["id"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quizMetadata["date"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quizMetadata["date"]?.Value<string>(), Does.Match(@"^\d{2}/\d{2}/\d{4} 00:00:00$"));
        Assert.That(quizMetadata["title"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quizMetadata["url"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quizMetadata["source"]?.Value<string>(), Is.EqualTo("API").Or.EqualTo("RSS"));
    }

    [Test]
    public async Task GivenValidQuizMetadataRequestWithCount_WhenRequestIsMade_ThenExpectedNumberOfItemsIsReceived()
    {
        // Given
        const int expectedCount = 7;
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = $"api/quiz-metadata",
            Query = $"count={expectedCount}"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Application.Json));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Not.Null.Or.Empty);

        var quizMetadataArray = JArray.Parse(content);
        Assert.That(quizMetadataArray.Count, Is.EqualTo(expectedCount));
    }
}