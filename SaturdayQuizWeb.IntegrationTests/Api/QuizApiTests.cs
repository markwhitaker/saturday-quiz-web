using Mainwave.MimeTypes;
using Newtonsoft.Json.Linq;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.IntegrationTests.Api;

[TestFixture]
public class QuizApiTests
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        DateParseHandling = DateParseHandling.None
    };

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

        var quiz = JsonConvert.DeserializeObject(content, JsonSerializerSettings) as JObject;
        Assert.That(quiz, Is.Not.Null);

        Assert.That(quiz!.ContainsKey("id"));
        Assert.That(quiz["id"]?.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quiz.ContainsKey("date"));
        var dateValue = quiz["date"]!.Value<string>();
        Assert.That(dateValue, Is.Not.Null.Or.Empty);
        Assert.That(dateValue, Does.Match(@"^\d{4}-\d{2}-\d{2}T00:00:00Z$"));
        Assert.That(DateTime.TryParse(dateValue, out var date), Is.True);
        Assert.That(date.Date, Is.InRange(DateTime.Today.Subtract(TimeSpan.FromDays(7)), DateTime.Today));

        Assert.That(quiz.ContainsKey("title"));
        Assert.That(quiz["title"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quiz.ContainsKey("questions"));
        Assert.That(quiz["questions"], Is.Not.Null.Or.Empty);

        var questions = quiz["questions"] as JArray;
        Assert.That(questions, Is.Not.Null);
        Assert.That(questions!.Count, Is.EqualTo(15));

        var normalQuestion = questions[0] as JObject;
        Assert.That(normalQuestion, Is.Not.Null);

        Assert.That(normalQuestion!.ContainsKey("number"));
        Assert.That(normalQuestion["number"]!.Value<int>(), Is.EqualTo(1));

        Assert.That(normalQuestion.ContainsKey("question"));
        Assert.That(normalQuestion["question"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(normalQuestion.ContainsKey("answer"));
        Assert.That(normalQuestion["answer"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(normalQuestion.ContainsKey("type"));
        Assert.That(normalQuestion["type"]!.Value<string>(), Is.EqualTo("NORMAL"));

        var whatLinksQuestion = questions[8] as JObject;
        Assert.That(whatLinksQuestion, Is.Not.Null);

        Assert.That(whatLinksQuestion!.ContainsKey("number"));
        Assert.That(whatLinksQuestion["number"]!.Value<int>(), Is.EqualTo(9));

        Assert.That(whatLinksQuestion.ContainsKey("type"));
        Assert.That(whatLinksQuestion["type"]!.Value<string>(), Is.EqualTo("WHAT_LINKS"));
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

        var quiz = JsonConvert.DeserializeObject(content, JsonSerializerSettings) as JObject;
        Assert.That(quiz, Is.Not.Null);

        Assert.That(quiz!.ContainsKey("id"));
        Assert.That(quiz["id"]?.Value<string>(), Is.EqualTo(expectedId));

        Assert.That(quiz.ContainsKey("date"));
        Assert.That(quiz["date"]?.Value<string>(), Is.Not.Null.Or.Empty);
        Assert.That(quiz["date"]?.Value<string>(), Does.Match(@"^\d{4}-\d{2}-\d{2}T00:00:00Z$"));
        Assert.That(DateTime.TryParse(quiz["date"]?.Value<string>(), out var date), Is.True);
        Assert.That(date.Date, Is.InRange(DateTime.Today.Subtract(TimeSpan.FromDays(7)), DateTime.Today));

        Assert.That(quiz.ContainsKey("title"));
        Assert.That(quiz["title"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(quiz.ContainsKey("questions"));
        Assert.That(quiz["questions"], Is.Not.Null.Or.Empty);

        var questions = quiz["questions"] as JArray;
        Assert.That(questions, Is.Not.Null);
        Assert.That(questions!.Count, Is.EqualTo(15));

        var normalQuestion = questions[0] as JObject;
        Assert.That(normalQuestion, Is.Not.Null);

        Assert.That(normalQuestion!.ContainsKey("number"));
        Assert.That(normalQuestion["number"]!.Value<int>(), Is.EqualTo(1));

        Assert.That(normalQuestion.ContainsKey("question"));
        Assert.That(normalQuestion["question"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(normalQuestion.ContainsKey("answer"));
        Assert.That(normalQuestion["answer"]!.Value<string>(), Is.Not.Null.Or.Empty);

        Assert.That(normalQuestion.ContainsKey("type"));
        Assert.That(normalQuestion["type"]!.Value<string>(), Is.EqualTo("NORMAL"));

        var whatLinksQuestion = questions[8] as JObject;
        Assert.That(whatLinksQuestion, Is.Not.Null);

        Assert.That(whatLinksQuestion!.ContainsKey("number"));
        Assert.That(whatLinksQuestion["number"]!.Value<int>(), Is.EqualTo(9));

        Assert.That(whatLinksQuestion.ContainsKey("type"));
        Assert.That(whatLinksQuestion["type"]!.Value<string>(), Is.EqualTo("WHAT_LINKS"));
    }

    [Test]
    public async Task GivenInvalidQuizRequest_WhenRequestIsMade_ThenNotFoundResponseIsReceived()
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz",
            Query = "id=invalid"
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        var content = await response.Content.ReadAsStringAsync();
        Assert.That(content, Is.Empty);
    }
}
