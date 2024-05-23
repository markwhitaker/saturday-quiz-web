using Microsoft.Extensions.Logging.Testing;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.IntegrationTests.Services
{
    [TestFixture]
    public class QuizMetadataServiceTests
    {
        private IQuizMetadataService _quizMetadataService = null!;

        [SetUp]
        public void SetUp()
        {
            var configOptions = ConfigOptionsLoader.ConfigOptions;
            var guardianWebsiteService = new GuardianWebsiteClient(new HttpClient(), configOptions);
            var guardianApiService = new GuardianApiClient(
                new HttpClient(),
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
        }

        [Test]
        public async Task TestGetQuizMetadata()
        {
            var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(7);
            Assert.That(quizMetadataList.Count, Is.EqualTo(7));
        }
    }
}
