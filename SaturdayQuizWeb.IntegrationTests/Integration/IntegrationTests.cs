using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using RestSharp;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.IntegrationTests.Integration
{
    [TestFixture]
    public class IntegrationTests
    {
        private readonly IQuizMetadataService _quizMetadataService;
        private readonly IQuizService _quizService;

        public IntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<IntegrationTests>()
                .Build();
            var configVariables = new ConfigVariables(configuration);
            var guardianApiHttpService = new GuardianApiHttpService(
                new RestClient("https://content.guardianapis.com/theguardian/"),
                configVariables);
            _quizMetadataService = new QuizMetadataService(guardianApiHttpService);
            _quizService = new QuizService(
                new GuardianScraperHttpService(new HttpClient()),
                new HtmlService(),
                _quizMetadataService);
        }

        [Test]
        public void TestGetQuizMetadata()
        {
            var request = _quizMetadataService.GetQuizMetadataAsync(7);
            request.Wait();
            Assert.AreEqual(7, request.Result.Count);
        }
        
        [Test]
        public async Task WhenLast50QuizzesAreLoaded_ThenAllAreSuccessful()
        {
            // When
            var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(50);
            var failedCount = 0;
            // Then
            for (var index = 0; index < 50; index++)
            {
                var quizMetadata = quizMetadataList[index];
                try
                {
                    var quiz = await _quizService.GetQuizAsync(quizMetadata.Id);
                    Console.WriteLine($"Index {index} successful");
                    PrintQuiz(quiz);
                }
                catch (Exception e)
                {
                    failedCount++;
                    Console.WriteLine($"Index {index} failed: {e.Message} ({quizMetadata.Url})");
                }
            }
            
            Assert.AreEqual(0, failedCount);
        }

        private static void PrintQuiz(Quiz quiz)
        {
            foreach (var q in quiz.Questions)
            {
                Console.WriteLine($"{q.Number}. {q.Type} {q.QuestionText} / {q.Answer}");
            }
            Console.WriteLine();
        }
    }
}
