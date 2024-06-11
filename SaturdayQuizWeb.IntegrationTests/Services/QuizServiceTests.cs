using Microsoft.Extensions.Logging.Testing;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.IntegrationTests.Services;

[TestFixture]
[Category("LongRunning")]
public class QuizServiceTests
{
    private IQuizMetadataService _quizMetadataService = null!;
    private IQuizService _quizService = null!;

    [SetUp]
    public void SetUp()
    {
        var configOptions = ConfigOptionsLoader.ConfigOptions;
        var guardianWebsiteService = new GuardianWebsiteHttpClient(new HttpClient(), configOptions);
        var guardianApiService = new GuardianApiClient(
            new GuardianApiHttpClient(new HttpClient(), configOptions),
            configOptions,
            new FakeLogger<GuardianApiClient>());
        var guardianRssService = new GuardianRssClient(
            configOptions,
            guardianWebsiteService,
            new FakeLogger<GuardianRssClient>());

        _quizMetadataService = new QuizMetadataService(
            new DateTimeWrapper(),
            guardianApiService,
            guardianRssService,
            new FakeLogger<QuizMetadataService>());

        _quizService = new QuizService(
            new DateTimeWrapper(),
            new GuardianWebsiteHttpClient(new HttpClient(), configOptions),
            new HtmlService(
                new SectionExtractor(),
                new HtmlStripper(),
                new SectionSplitter(),
                new QuestionAssembler()),
            _quizMetadataService);
    }

    [Test]
    public async Task WhenLast50QuizzesAreLoaded_ThenAllAreSuccessful()
    {
        // When
        const int expectedNumberOfQuizzes = 50;

        var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(expectedNumberOfQuizzes);
        var failedDates = new List<string>();

        Assert.That(quizMetadataList.Count, Is.EqualTo(expectedNumberOfQuizzes));

        // Then
        for (var index = 0; index < expectedNumberOfQuizzes; index++)
        {
            var quizMetadata = quizMetadataList[index];
            try
            {
                await _quizService.GetQuizAsync(quizMetadata.Id);
            }
            catch (Exception e)
            {
                var dateString = quizMetadata.Date.ToShortDateString();
                failedDates.Add(dateString);
                await TestContext.Out.WriteLineAsync($"Index {index} ({dateString}) failed: {e.Message} ({quizMetadata.Url})");
            }
        }

        Assert.That(failedDates, Is.Empty, $"Failed to parse {failedDates.Count} of the last {expectedNumberOfQuizzes} quizzes");
    }
}
