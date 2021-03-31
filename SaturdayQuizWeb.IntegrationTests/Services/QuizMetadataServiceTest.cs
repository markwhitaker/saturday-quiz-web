﻿using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RestSharp;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.IntegrationTests.Services
{
    [TestFixture]
    public class QuizMetadataServiceTest
    {
        private IQuizMetadataService _quizMetadataService;

        [SetUp]
        public void SetUp()
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
        public void TestGetQuizMetadata()
        {
            var request = _quizMetadataService.GetQuizMetadataAsync(7);
            request.Wait();
            Assert.AreEqual(7, request.Result.Count);
        }
    }
}
