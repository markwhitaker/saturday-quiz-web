using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.IntegrationTests.WebServer;

[TestFixture]
public class WebServerTests
{
    private HttpClient _httpClient = null!;

    [SetUp]
    public void SetUp() => _httpClient = new WebApplicationFactory<Program>().CreateClient();

    [TestCase("")]
    [TestCase("css/styles.css")]
    [TestCase("script/Presenter.js")]
    [TestCase("images/icon32.png")]
    [TestCase("api/quiz")]
    [TestCase("api/quiz-metadata")]
    public async Task GivenAnyUri_WhenRequested_ThenResponseStatusIsOK(string path)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();


        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TestCase("api/quiz")]
    [TestCase("api/quiz-metadata")]
    public async Task GivenApiUri_WhenRequested_ThenCacheControlHeaderValueIsNoCache(string path)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Headers.CacheControl, Is.Not.Null);
        Assert.That(response.Headers.CacheControl!.NoCache, Is.True);
    }

    [TestCase("")]
    [TestCase("css/styles.css")]
    [TestCase("script/Presenter.js")]
    [TestCase("images/icon32.png")]
    public async Task GivenStaticFileUri_WhenRequested_ThenCacheControlHeaderValueIs30DaysCache(string path)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Headers.CacheControl, Is.Not.Null);
        Assert.That(response.Headers.CacheControl!.NoCache, Is.False);
        Assert.That(response.Headers.CacheControl!.MaxAge, Is.EqualTo(TimeSpan.FromDays(30)));
    }

    [Test]
    public async Task GivenQuizApiUriWithId_WhenRequested_ThenCacheControlHeaderValueIs365Days()
    {
        // Given
        var metadataRequestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz-metadata",
            Query = "count=1"
        }.ToString();
        var metadataResponse = await _httpClient.GetAsync(metadataRequestUri);
        var metadataJson = await metadataResponse.Content.ReadAsStringAsync();
        var metadata = JsonConvert.DeserializeObject<QuizMetadata[]>(metadataJson)!.First();

        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = "api/quiz",
            Query = $"id={metadata.Id}"
        }.ToString();
        var expectedCacheControlMaxAge = TimeSpan.FromDays(365);

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Headers.CacheControl, Is.Not.Null);
        Assert.That(response.Headers.CacheControl!.MaxAge, Is.EqualTo(expectedCacheControlMaxAge));
    }

    [TestCase("")]
    [TestCase("css/styles.css")]
    [TestCase("script/Presenter.js")]
    [TestCase("images/icon32.png")]
    [TestCase("api/quiz")]
    [TestCase("api/quiz-metadata")]
    public async Task GivenAnyUri_WhenRequested_ThenXContentTypeOptionsHeaderValueIsNosniff(string path)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Headers.Contains("X-Content-Type-Options"), Is.True);
        Assert.That(response.Headers.GetValues("X-Content-Type-Options"), Has.One.Items.EqualTo("nosniff"));
    }

    [TestCase("", "text/html")]
    [TestCase("css/styles.css", "text/css")]
    [TestCase("script/Presenter.js", "text/javascript")]
    public async Task GivenTextResourceUri_WhenRequested_ThenContentTypeHeaderValueHasCharsetUtf8(
        string path,
        string expectedContentType)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Content.Headers.Contains("Content-Type"), Is.True);
        Assert.That(response.Content.Headers.GetValues("Content-Type"), Has.One.Items.EqualTo($"{expectedContentType}; charset=utf-8"));
    }

    [TestCase("images/icon32.png", "image/png")]
    public async Task GivenNonTextResourceUri_WhenRequested_ThenContentTypeHeaderValueDoesNotHaveCharsetUtf8(
        string path,
        string expectedContentType)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Content.Headers.Contains("Content-Type"), Is.True);
        Assert.That(response.Content.Headers.GetValues("Content-Type"), Has.One.Items.EqualTo(expectedContentType));
    }

    [TestCase("api/quiz")]
    [TestCase("api/quiz-metadata")]
    public async Task GivenApiUri_WhenRequested_ThenContentTypeHeaderValueIsApplicationJsonCharsetUtf8(string path)
    {
        // Given
        var requestUri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri)
        {
            Path = path
        }.ToString();

        // When
        var response = await _httpClient.GetAsync(requestUri);

        // Then
        Assert.That(response.Content.Headers.Contains("Content-Type"), Is.True);
        Assert.That(response.Content.Headers.GetValues("Content-Type"), Has.One.Items.EqualTo("application/json; charset=utf-8"));
    }
}
