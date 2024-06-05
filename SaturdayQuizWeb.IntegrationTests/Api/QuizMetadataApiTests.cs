using Mainwave.MimeTypes;
using Newtonsoft.Json.Linq;

namespace SaturdayQuizWeb.IntegrationTests.Api;

[TestFixture]
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

        var jsonSerializerSettings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None
        };

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Application.Json));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Not.Null.Or.Empty);

        var quizMetadataArray = JsonConvert.DeserializeObject(content, jsonSerializerSettings) as JArray;
        Assert.That(quizMetadataArray, Is.Not.Null);
        Assert.That(quizMetadataArray!.Count, Is.EqualTo(expectedCount));

        var quizMetadata = quizMetadataArray.First as JObject;
        Assert.That(quizMetadata, Is.Not.Null);

        Assert.That(quizMetadata!.ContainsKey("id"));
        Assert.That(quizMetadata["id"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quizMetadata.ContainsKey("date"));
        var dateValue = quizMetadata["date"]!.Value<string>();
        Assert.That(dateValue, Is.Not.Null.Or.Empty);
        Assert.That(dateValue, Does.Match(@"^\d{4}-\d{2}-\d{2}T00:00:00Z$"));
        Assert.That(DateTime.TryParse(dateValue, out var date), Is.True);
        Assert.That(date.Date, Is.InRange(DateTime.Today.Subtract(TimeSpan.FromDays(7)), DateTime.Today));

        Assert.That(quizMetadata.ContainsKey("title"));
        Assert.That(quizMetadata["title"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quizMetadata.ContainsKey("url"));
        Assert.That(quizMetadata["url"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quizMetadata.ContainsKey("source"));
        Assert.That(quizMetadata["source"]!.Value<string>(), Is.EqualTo("API").Or.EqualTo("RSS"));
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
