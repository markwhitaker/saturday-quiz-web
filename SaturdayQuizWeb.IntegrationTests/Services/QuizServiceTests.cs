using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.IntegrationTests.Services
{
    [TestFixture]
    public class QuizServiceTests
    {
        private IQuizMetadataService _quizMetadataService = null!;
        private IQuizService _quizService = null!;

        [SetUp]
        public void SetUp()
        {
            var configOptions = ConfigOptionsLoader.ConfigOptions;
            var guardianWebsiteService = new GuardianWebsiteClient(new HttpClient(), configOptions);
            var guardianApiService = new GuardianApiClient(new HttpClient(), configOptions);
            var guardianRssService = new GuardianRssClient(configOptions, guardianWebsiteService);

            _quizMetadataService = new QuizMetadataService(
                new DateTimeWrapper(),
                guardianApiService,
                guardianRssService);

            _quizService = new QuizService(
                new DateTimeWrapper(),
                new GuardianWebsiteClient(new HttpClient(), configOptions),
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
                    var quiz = await _quizService.GetQuizAsync(quizMetadata.Id);
                    await TestContext.Out.WriteLineAsync($"Index {index} successful");
                    await PrintQuizAsync(quiz);
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

        private static async Task PrintQuizAsync(Quiz quiz)
        {
            foreach (var q in quiz.Questions)
            {
                await TestContext.Out.WriteLineAsync($"{q.Number} [{q.Type}] {q.Question} {q.Answer}");
            }

            await TestContext.Out.WriteLineAsync();
        }
    }
}
