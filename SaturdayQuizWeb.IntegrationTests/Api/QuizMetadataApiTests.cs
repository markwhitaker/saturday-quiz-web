using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Nodes;
using Mainwave.MimeTypes;

namespace SaturdayQuizWeb.IntegrationTests.Api;

[TestFixture]
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class QuizMetadataApiTests
{
    private HttpClient _httpClient = null!;

    [SetUp]
    public void SetUp() => _httpClient = new WebApplicationFactory<Program>().CreateClient();

    [Test]
    public async Task GivenValidQuizMetadataRequestWithoutCount_WhenRequestIsMade_ThenExpectedResponseIsReceived()
    {
        // Given
        const int expectedCount = 10;
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

        var quizMetadataArray = JsonNode.Parse(content) as JsonArray;
        Assert.That(quizMetadataArray, Is.Not.Null);
        Assert.That(quizMetadataArray!.Count, Is.EqualTo(expectedCount));

        var quizMetadata = quizMetadataArray.First() as JsonObject;
        Assert.That(quizMetadata, Is.Not.Null);

        Assert.That(quizMetadata!.ContainsKey("id"));
        Assert.That(quizMetadata["id"]!.GetValue<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quizMetadata.ContainsKey("date"));
        var dateValue = quizMetadata["date"]!.GetValue<string>();
        Assert.That(dateValue, Is.Not.Null.Or.Empty);
        Assert.That(dateValue, Does.Match(@"^\d{4}-\d{2}-\d{2}T00:00:00Z$"));
        Assert.That(DateTime.TryParse(dateValue, out var date), Is.True);
        Assert.That(date.Date, Is.InRange(DateTime.Today.Subtract(TimeSpan.FromDays(7)), DateTime.Today));

        Assert.That(quizMetadata.ContainsKey("title"));
        Assert.That(quizMetadata["title"]!.GetValue<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quizMetadata.ContainsKey("url"));
        Assert.That(quizMetadata["url"]!.GetValue<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quizMetadata.ContainsKey("source"));
        Assert.That(quizMetadata["source"]!.GetValue<string>(), Is.EqualTo("API").Or.EqualTo("RSS"));

        Assert.That(quizMetadata.ContainsKey("apiUrl"));
        var apiUrlValue = quizMetadata["apiUrl"]!.GetValue<string>();
        var expectedApiUrl = new Uri(_httpClient.BaseAddress!, $"/api/quiz/{date:yyyy-MM-dd}").AbsoluteUri;
        Assert.That(apiUrlValue, Is.EqualTo(expectedApiUrl));
    }

    [Test]
    public async Task GivenValidQuizMetadataRequestWithCount_WhenRequestIsMade_ThenExpectedNumberOfItemsIsReceived()
    {
        // Given
        const int expectedCount = 7;
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz-metadata",
            Query = $"count={expectedCount}"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Application.Json));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Not.Null.Or.Empty);

        var quizMetadataArray = JsonNode.Parse(content) as JsonArray;
        Assert.That(quizMetadataArray!.Count, Is.EqualTo(expectedCount));
    }
}
