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
[TestOf(typeof(GuardianApiClient))]
public class GuardianApiClientTests
{
    private IGuardianApiHttpClient _mockGuardianApiHttpClient = null!;
    private IOptions<GuardianConfig> _mockConfigOptions = null!;
    private ILogger<GuardianApiClient> _mockLogger = null!;

    private IGuardianApiClient _guardianApiClient = null!;

    [SetUp]
    public void SetUp()
    {
        _mockGuardianApiHttpClient = Substitute.For<IGuardianApiHttpClient>();
        _mockConfigOptions = Substitute.For<IOptions<GuardianConfig>>();
        _mockLogger = Substitute.For<ILogger<GuardianApiClient>>();
        _guardianApiClient = new GuardianApiClient(_mockGuardianApiHttpClient, _mockConfigOptions, _mockLogger);
    }

    [Test]
    public async Task GivenValidUrl_WhenGetQuizMetadataAsync_ThenExpectedMetadataIsReturned()
    {
        // Given
        const string apiEndpoint = "api-endpoint";
        const string apiKey = "api-key";
        const int expectedCount = 3;
        const string expectedUrl = "api-endpoint?api-key=api-key&page-size=3";

        var expectedConfig = new GuardianConfig
        {
            ApiEndpoint = apiEndpoint,
            ApiKey = apiKey
        };

        var testJsonFilePath = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "TestData",
            "metadata-api-response.json");
        var testJson = await File.ReadAllTextAsync(testJsonFilePath);

        _mockConfigOptions.Value.Returns(expectedConfig);
        _mockGuardianApiHttpClient.GetStringAsync(expectedUrl)
            .Returns(Task.FromResult(testJson));

        // When
        var actualMetadata = await _guardianApiClient.GetQuizMetadataAsync(expectedCount);

        // Then
        Assert.That(actualMetadata, Has.Exactly(expectedCount).Items);
        var firstMetadata = actualMetadata[0];
        Assert.That(firstMetadata.Id, Is.EqualTo("games/article/2024/may/25/which-technology-was-popularised-by-chers-hit-believe-the-saturday-quiz"));
        Assert.That(firstMetadata.Title, Is.EqualTo("Which technology was popularised by Cher’s hit Believe? The Saturday quiz"));
        Assert.That(firstMetadata.Date, Is.EqualTo(new DateTime(2024, 5, 25, 0, 0, 0, DateTimeKind.Utc)));
        Assert.That(firstMetadata.Source, Is.EqualTo(Constants.SourceApi));
        Assert.That(firstMetadata.Url, Is.EqualTo("https://www.theguardian.com/games/article/2024/may/25/which-technology-was-popularised-by-chers-hit-believe-the-saturday-quiz"));
    }

    [Test]
    public async Task GivenInvalidUrl_WhenGetQuizMetadataAsync_ThenEmptyListIsReturned()
    {
        // Given
        var expectedConfig = new GuardianConfig
        {
            ApiEndpoint = string.Empty,
            ApiKey = string.Empty
        };

        _mockConfigOptions.Value.Returns(expectedConfig);
        _mockGuardianApiHttpClient
            .GetStringAsync(Arg.Any<string>())
            .ThrowsAsync<HttpRequestException>();

        // When
        var actualMetadata = await _guardianApiClient.GetQuizMetadataAsync(default);

        // Then
        Assert.That(actualMetadata, Is.Empty);
    }
}
