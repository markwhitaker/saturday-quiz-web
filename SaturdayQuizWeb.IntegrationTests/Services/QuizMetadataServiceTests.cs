using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;
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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .AddUserSecrets<QuizMetadataServiceTests>()
                .Build();

            var services = new ServiceCollection()
                .Configure<GuardianConfig>(configuration.GetSection(Constants.ConfigSectionGuardian))
                .BuildServiceProvider();

            var configOptions = services.GetService<IOptions<GuardianConfig>>() ?? throw new Exception(
                $"Failed to get IOptions<{nameof(GuardianConfig)}> from service provider");

            var guardianWebsiteService = new GuardianWebsiteClient(new HttpClient(), configOptions);
            var guardianApiService = new GuardianApiClient(configOptions);
            var guardianRssService = new GuardianRssClient(configOptions, guardianWebsiteService);
            _quizMetadataService = new QuizMetadataService(
                new DateTimeWrapper(),
                guardianApiService,
                guardianRssService);
        }

        [Test]
        public async Task TestGetQuizMetadata()
        {
            var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(7);
            Assert.That(quizMetadataList.Count, Is.EqualTo(7));
        }
    }
}
