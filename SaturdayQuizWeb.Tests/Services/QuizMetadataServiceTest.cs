using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RestSharp;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Tests.Services
{
    [TestFixture]
    public class QuizMetadataServiceTest
    {
        // Object under test
        private readonly IQuizMetadataService _quizMetadataService;

        public QuizMetadataServiceTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<QuizMetadataServiceTest>()
                .Build();
            var configVariables = new ConfigVariables(configuration);
            var guardianApiHttpService = new GuardianApiHttpService(
                new RestClient("https://content.guardianapis.com/theguardian/"),
                configVariables);
            _quizMetadataService = new QuizMetadataService(guardianApiHttpService);
        }

        [Test]
        [Ignore(Consts.IntegrationTest)]
        public void TestGetQuizMetadata()
        {
            var request = _quizMetadataService.GetQuizMetadataAsync(7);
            request.Wait();
            Assert.AreEqual(7, request.Result.Count);
        }
    }
}
