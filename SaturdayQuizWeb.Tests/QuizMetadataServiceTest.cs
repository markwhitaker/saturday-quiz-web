using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SaturdayQuizWeb.Api;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Tests
{
    public class QuizMetadataServiceTest
    {
        private IConfiguration Configuration { get; }

        private readonly IQuizMetadataService _service = new QuizMetadataService(new GuardianApi());
        private readonly IConfigVariables _configVariables;

        public QuizMetadataServiceTest()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<QuizMetadataServiceTest>()
                .Build();
            _configVariables = new ConfigVariables(Configuration);
        }

        [Test]
        [Ignore(Consts.IntegrationTest)]
        public void TestGetQuizMetadata()
        {
            var request = _service.GetQuizMetadata(_configVariables.GuardianApiKey, 7);
            request.Wait();
            Assert.AreEqual(7, request.Result.Count);
        }
    }
}
