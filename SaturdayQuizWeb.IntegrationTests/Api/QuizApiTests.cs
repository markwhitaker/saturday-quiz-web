using Mainwave.MimeTypes;
using Newtonsoft.Json.Linq;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.IntegrationTests.Api;

[TestFixture]
public class QuizApiTests
{
    private HttpClient _httpClient = null!;

    [SetUp]
    public void SetUp() => _httpClient = new WebApplicationFactory<Program>().CreateClient();

    [Test]
    public async Task GivenValidQuizRequestWithoutId_WhenRequestIsMade_ThenExpectedResponseIsReceived()
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Application.Json));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Not.Null.Or.Empty);

        var quiz = JObject.Parse(content);
        Assert.That(quiz["id"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quiz["date"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quiz["date"]?.Value<string>(), Does.Match(@"^\d{2}/\d{2}/\d{4} 00:00:00$"));
        Assert.That(quiz["title"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quiz["questions"], Is.Not.Null.Or.Empty);

        var questions = quiz["questions"]!;
        Assert.That(questions.Count(), Is.EqualTo(15));

        var normalQuestion = questions[0]!;
        Assert.That(normalQuestion["number"]?.Value<int>(), Is.EqualTo(1));
        Assert.That(normalQuestion["question"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(normalQuestion["answer"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(normalQuestion["type"]?.Value<string>(), Is.EqualTo("NORMAL"));

        var whatLinksQuestion = questions[8]!;
        Assert.That(whatLinksQuestion["number"]?.Value<int>(), Is.EqualTo(9));
        Assert.That(whatLinksQuestion["type"]?.Value<string>(), Is.EqualTo("WHAT_LINKS"));
    }

    [Test]
    public async Task GivenValidQuizRequestWithId_WhenRequestIsMade_ThenExpectedResponseIsReceived()
    {
        // Given
        var metadataRequestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz-metadata",
            Query = "count=1"
        }.ToString();
        var metadataJson = await _httpClient.GetStringAsync(metadataRequestUri);
        var metadata = JsonConvert.DeserializeObject<QuizMetadata[]>(metadataJson);
        var expectedId = metadata![0].Id;

        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz/",
            Query = $"id={expectedId}"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Content.Headers.ContentType!.MediaType, Is.EqualTo(MimeType.Application.Json));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Not.Null.Or.Empty);

        var quiz = JObject.Parse(content);
        Assert.That(quiz["id"]?.Value<string>(), Is.EqualTo(expectedId));
        Assert.That(quiz["date"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quiz["date"]?.Value<string>(), Does.Match(@"^\d{2}/\d{2}/\d{4} 00:00:00$"));
        Assert.That(quiz["title"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quiz["questions"], Is.Not.Null.Or.Empty);

        var questions = quiz["questions"]!;
        Assert.That(questions.Count(), Is.EqualTo(15));

        var normalQuestion = questions[0]!;
        Assert.That(normalQuestion["number"]?.Value<int>(), Is.EqualTo(1));
        Assert.That(normalQuestion["question"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(normalQuestion["answer"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(normalQuestion["type"]?.Value<string>(), Is.EqualTo("NORMAL"));

        var whatLinksQuestion = questions[8]!;
        Assert.That(whatLinksQuestion["number"]?.Value<int>(), Is.EqualTo(9));
        Assert.That(whatLinksQuestion["type"]?.Value<string>(), Is.EqualTo("WHAT_LINKS"));
    }

    [Test]
    public async Task GivenInvalidQuizRequest_WhenRequestIsMade_ThenNotFoundResponseIsReceived()
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz/invalid"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Empty);
    }
}
