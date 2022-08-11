using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using RestSharp;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.IntegrationTests.Services
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
            
            var services = new ServiceCollection()
                .Configure<SaturdayQuizConfig>(configuration)
                .BuildServiceProvider();
            
            var configOptions = services.GetService<IOptions<SaturdayQuizConfig>>();
            
            var guardianApiHttpService = new GuardianApiHttpService(
                new RestClient("https://content.guardianapis.com/theguardian/"),
                configOptions!);
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
