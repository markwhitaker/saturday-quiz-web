using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using RestSharp;
using SaturdayQuizWeb.Config;
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
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<QuizServiceTests>()
                .Build();

            var services = new ServiceCollection()
                .Configure<SaturdayQuizConfig>(configuration)
                .BuildServiceProvider();

            var configOptions = services.GetService<IOptions<SaturdayQuizConfig>>();

            var guardianApiHttpService = new GuardianApiHttpService(
                new RestClient("https://content.guardianapis.com/theguardian/"),
                configOptions!);

            _quizMetadataService = new QuizMetadataService(guardianApiHttpService);
            _quizService = new QuizService(
                new DateTimeWrapper(),
                new GuardianScraperHttpService(new HttpClient()),
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

            // Then
            for (var index = 0; index < expectedNumberOfQuizzes; index++)
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
                    var dateString = quizMetadata.Date.ToShortDateString();
                    failedDates.Add(dateString);
                    Console.WriteLine($"Index {index} ({dateString}) failed: {e.Message} ({quizMetadata.Url})");
                }
            }

            Assert.That(failedDates, Is.Empty, "Failed to parse {0} of the last {1} quizzes", failedDates.Count, expectedNumberOfQuizzes);
        }

        private static void PrintQuiz(Quiz quiz)
        {
            foreach (var q in quiz.Questions)
            {
                Console.WriteLine($"{q.Number} [{q.Type}] {q.Question} {q.Answer}");
            }

            Console.WriteLine();
        }
    }
}
