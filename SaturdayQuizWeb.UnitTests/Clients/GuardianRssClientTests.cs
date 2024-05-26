using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute.ExceptionExtensions;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.UnitTests.Clients;

[TestFixture]
[TestOf(typeof(GuardianRssClient))]
public class GuardianRssClientTests
{
    private IGuardianWebsiteHttpClient _mockGuardianWebsiteHttpClient = null!;
    private IOptions<GuardianConfig> _mockConfigOptions = null!;
    private ILogger<GuardianRssClient> _mockLogger = null!;

    private IGuardianRssClient _guardianRssClient = null!;

    [SetUp]
    public void SetUp()
    {
        _mockGuardianWebsiteHttpClient = Substitute.For<IGuardianWebsiteHttpClient>();
        _mockConfigOptions = Substitute.For<IOptions<GuardianConfig>>();
        _mockLogger = Substitute.For<ILogger<GuardianRssClient>>();
        _guardianRssClient = new GuardianRssClient(_mockConfigOptions, _mockGuardianWebsiteHttpClient, _mockLogger);
    }

    [Test]
    public async Task GivenValidUrl_WhenGetQuizMetadataAsync_ThenExpectedMetadataIsReturned()
    {
        // Given
        const string rssEndpoint = "rss-endpoint";
        const string websiteBaseUrl = "https://www.theguardian.com/";
        const int expectedCount = 3;

        var expectedConfig = new GuardianConfig
        {
            RssEndpoint = rssEndpoint,
            WebsiteBaseUrl = websiteBaseUrl
        };

        var testXmlFilePath = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "TestData",
            "metadata-rss-response.xml");
        var testXml = await File.ReadAllTextAsync(testXmlFilePath);

        _mockConfigOptions.Value.Returns(expectedConfig);
        _mockGuardianWebsiteHttpClient.GetStringAsync(rssEndpoint)
            .Returns(Task.FromResult(testXml));

        // When
        var actualMetadata = await _guardianRssClient.GetQuizMetadataAsync(expectedCount);

        // Then
        Assert.That(actualMetadata, Has.Exactly(expectedCount).Items);
        var firstMetadata = actualMetadata[0];
        Assert.That(firstMetadata.Id, Is.EqualTo("games/article/2024/may/25/which-technology-was-popularised-by-chers-hit-believe-the-saturday-quiz"));
        Assert.That(firstMetadata.Title, Is.EqualTo("Which technology was popularised by Cher’s hit Believe? The Saturday quiz"));
        Assert.That(firstMetadata.Date, Is.EqualTo(new DateTime(2024, 5, 25, 0, 0, 0, DateTimeKind.Utc)));
        Assert.That(firstMetadata.Source, Is.EqualTo(Constants.SourceRss));
        Assert.That(firstMetadata.Url, Is.EqualTo("https://www.theguardian.com/games/article/2024/may/25/which-technology-was-popularised-by-chers-hit-believe-the-saturday-quiz"));
    }

    [Test]
    public async Task GivenInvalidUrl_WhenGetQuizMetadataAsync_ThenEmptyListIsReturned()
    {
        // Given
        const string rssEndpoint = "rss-endpoint";
        const int expectedCount = 3;

        var expectedConfig = new GuardianConfig
        {
            RssEndpoint = string.Empty,
            WebsiteBaseUrl = string.Empty
        };

        _mockConfigOptions.Value.Returns(expectedConfig);
        _mockGuardianWebsiteHttpClient
            .GetStringAsync(rssEndpoint)
            .ThrowsAsync<HttpRequestException>();

        // When
        var actualMetadata = await _guardianRssClient.GetQuizMetadataAsync(expectedCount);

        // Then
        Assert.That(actualMetadata, Is.Empty);
    }
}
