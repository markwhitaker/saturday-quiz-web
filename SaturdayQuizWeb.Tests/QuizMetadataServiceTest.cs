using System.Net.Http;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SaturdayQuizWeb.Api;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Tests
{
    public class QuizMetadataServiceTest
    {
        private static readonly IHttpClientFactory HttpClientFactory = new HttpClientFactory();
        private static readonly IGuardianApi GuardianApi = new GuardianApi(HttpClientFactory);

        private readonly IQuizMetadataService _service = new QuizMetadataService(GuardianApi);
        private readonly IConfigVariables _configVariables;

        public QuizMetadataServiceTest()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets<QuizMetadataServiceTest>()
                .Build();
            _configVariables = new ConfigVariables(configuration);
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

    internal class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new HttpClient();
    }
}
