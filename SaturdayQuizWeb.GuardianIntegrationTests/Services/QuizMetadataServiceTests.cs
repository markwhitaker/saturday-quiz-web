using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RestSharp;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.GuardianIntegrationTests.Services
{
    [TestFixture]
    public class QuizMetadataServiceTests
    {
        private IQuizMetadataService _quizMetadataService = null!;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<QuizMetadataServiceTests>()
                .Build();
            var configVariables = new ConfigVariables(configuration);
            var guardianApiHttpService = new GuardianApiHttpService(
                new RestClient("https://content.guardianapis.com/theguardian/"),
                configVariables);
            _quizMetadataService = new QuizMetadataService(guardianApiHttpService);
        }

        [Test]
        public async Task TestGetQuizMetadata()
        {
            var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(7);
            Assert.That(quizMetadataList.Count, Is.EqualTo(7));
        }
    }
}
