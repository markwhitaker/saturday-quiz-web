using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.UnitTests.Services;

[TestFixture]
public class QuizMetadataServiceTests
{
    private IDateTimeWrapper _mockDateTimeWrapper = null!;
    private IGuardianApiClient _mockGuardianApiClient = null!;
    private IGuardianRssClient _mockGuardianRssClient = null!;

    private IQuizMetadataService _quizMetadataService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockDateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        _mockGuardianApiClient = Substitute.For<IGuardianApiClient>();
        _mockGuardianRssClient = Substitute.For<IGuardianRssClient>();

        _quizMetadataService = new QuizMetadataService(
            _mockDateTimeWrapper,
            _mockGuardianApiClient,
            _mockGuardianRssClient);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(6)]
    public async Task GivenLatestQuizReturnedByApiIsLessThanAWeekOld_WhenGettingMetadata_ThenRssIsNotUsed(int ageInDays)
    {
        // Given
        const int quizzesRequestedCount = 5;
        var today = new DateTime(2022, 1, 8, 23, 59, 59);
        var oldDate = today.Subtract(TimeSpan.FromDays(ageInDays));

        var apiMetadata1 = new QuizMetadata
        {
            Id = "api-id-1",
            Date = oldDate,
            Title = "api-title-1",
            Url = "url-1",
            Source = "API"
        };
        var expectedApiResponse = new HashSet<QuizMetadata>
        {
            apiMetadata1
        };
        var rssMetadata1 = new QuizMetadata
        {
            Id = "rss-id-1",
            Date = today,
            Title = "rss-title-1",
            Url = "url-1",
            Source = "RSS"
        };
        var rssMetadata2 = new QuizMetadata
        {
            Id = "rss-id-2",
            Date = oldDate,
            Title = "rss-title-2",
            Url = "url-2",
            Source = "RSS"
        };
        var expectedRssResponse = new HashSet<QuizMetadata>
        {
            rssMetadata1,
            rssMetadata2
        };
        var expectedMetadataServiceResponse = new List<QuizMetadata>
        {
            apiMetadata1
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedApiResponse);
        _mockGuardianRssClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedRssResponse);

        // When
        var actualMetadataServiceResponse = await _quizMetadataService.GetQuizMetadataAsync(quizzesRequestedCount);

        // Then
        await _mockGuardianApiClient.Received().GetQuizMetadataAsync(quizzesRequestedCount);
        await _mockGuardianRssClient.DidNotReceiveWithAnyArgs().GetQuizMetadataAsync(default);
        Assert.That(expectedMetadataServiceResponse, Is.EqualTo(actualMetadataServiceResponse));
    }

    [TestCase(7)]
    [TestCase(8)]
    [TestCase(14)]
    [TestCase(9999)]
    public async Task GivenLatestQuizReturnedByApiIsAWeekOldOrOlder_WhenGettingMetadata_ThenRssIsUsed(int ageInDays)
    {
        // Given
        const int quizzesRequestedCount = 5;
        var today = new DateTime(2022, 1, 8, 23, 59, 59);
        var oldDate = today.Subtract(TimeSpan.FromDays(ageInDays));
        var apiMetadata2 = new QuizMetadata
        {
            Id = "api-id-2",
            Date = oldDate,
            Title = "api-title-2",
            Url = "url-2",
            Source = "API"
        };
        var expectedApiResponse = new HashSet<QuizMetadata>
        {
            apiMetadata2
        };
        var rssMetadata1 = new QuizMetadata
        {
            Id = "rss-id-1",
            Date = today,
            Title = "rss-title-1",
            Url = "url-1",
            Source = "RSS"
        };
        var rssMetadata2 = new QuizMetadata
        {
            Id = "rss-id-2",
            Date = oldDate,
            Title = "rss-title-2",
            Url = "url-2",
            Source = "RSS"
        };
        var expectedRssResponse = new HashSet<QuizMetadata>
        {
            rssMetadata1,
            rssMetadata2
        };
        var expectedMetadataServiceResponse = new List<QuizMetadata>
        {
            rssMetadata1,
            apiMetadata2
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedApiResponse);
        _mockGuardianRssClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedRssResponse);

        // When
        var actualMetadataServiceResponse = await _quizMetadataService.GetQuizMetadataAsync(quizzesRequestedCount);

        // Then
        await _mockGuardianApiClient.Received().GetQuizMetadataAsync(quizzesRequestedCount);
        await _mockGuardianRssClient.Received().GetQuizMetadataAsync(quizzesRequestedCount);
        Assert.That(expectedMetadataServiceResponse, Is.EqualTo(actualMetadataServiceResponse));
    }
}
