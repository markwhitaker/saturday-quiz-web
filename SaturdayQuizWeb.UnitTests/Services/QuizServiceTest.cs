using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.UnitTests.Services
{
    [TestFixture]
    public class QuizServiceTest
    {
        // Constants
        private const string TestQuizId = "test quiz id";
        private static readonly DateTime TestQuizDate = DateTime.UtcNow;
        private const string TestQuizTitle = "test quiz title";
        private const string TestQuizUrl = "test quiz url";
        private const int TestQuestionNumber = 1;
        private const QuestionType TestQuestionType = QuestionType.Normal;
        private const string TestQuestionText = "test question text";
        private const string TestQuestionAnswer = "test question answer";
        private const string TestHtmlContent = "test html content";
        private static readonly QuizMetadata QuizMetadata = new QuizMetadata
        {
            Id = TestQuizId,
            Date = TestQuizDate,
            Title = TestQuizTitle,
            Url = TestQuizUrl
        };
        private static readonly List<Question> Questions = new List<Question>
        {
            new Question
            {
                Number = TestQuestionNumber,
                Type = TestQuestionType,
                QuestionText = TestQuestionText,
                Answer = TestQuestionAnswer
            }
        };

        // Mocks
        private static readonly Mock<IGuardianScraperHttpService> MockScraperHttpService = new Mock<IGuardianScraperHttpService>();
        private static readonly Mock<IHtmlService> MockHtmlService = new Mock<IHtmlService>();
        private static readonly Mock<IQuizMetadataService> MockQuizMetadataService = new Mock<IQuizMetadataService>();
        
        // Object under test
        private readonly IQuizService _quizService = new QuizService(
            MockScraperHttpService.Object,
            MockHtmlService.Object,
            MockQuizMetadataService.Object);
        
        [Test]
        public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncByMetadata_ThenExpectedQuizReturned()
        {
            // Given
            MockScraperHttpService
                .Setup(service => service.GetQuizPageContentAsync(TestQuizId))
                .ReturnsAsync(TestHtmlContent);
            MockHtmlService
                .Setup(service => service.FindQuestions(TestHtmlContent))
                .Returns(Questions);
            // When
            var quiz = await _quizService.GetQuizAsync(QuizMetadata);
            // Then
            Assert.AreEqual(TestQuizId, quiz.Id);
            Assert.AreEqual(TestQuizDate, quiz.Date);
            Assert.AreEqual(TestQuizTitle, quiz.Title);
            Assert.AreEqual(Questions, quiz.Questions);
        }
        
        [Test]
        public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncWithNullId_ThenExpectedQuizReturned()
        {
            // Given
            MockQuizMetadataService
                .Setup(service => service.GetQuizMetadataAsync(1))
                .ReturnsAsync(new List<QuizMetadata>
                {
                    QuizMetadata
                });
            MockScraperHttpService
                .Setup(service => service.GetQuizPageContentAsync(TestQuizId))
                .ReturnsAsync(TestHtmlContent);
            MockHtmlService
                .Setup(service => service.FindQuestions(TestHtmlContent))
                .Returns(Questions);
            // When
            var quiz = await _quizService.GetQuizAsync();
            // Then
            Assert.AreEqual(TestQuizId, quiz.Id);
            Assert.AreEqual(TestQuizDate, quiz.Date);
            Assert.AreEqual(TestQuizTitle, quiz.Title);
            Assert.AreEqual(Questions, quiz.Questions);
        }
        
        [Test]
        public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncWithNonNullId_ThenExpectedQuizReturned()
        {
            // Given
            MockScraperHttpService
                .Setup(service => service.GetQuizPageContentAsync(TestQuizId))
                .ReturnsAsync(TestHtmlContent);
            MockHtmlService
                .Setup(service => service.FindQuestions(TestHtmlContent))
                .Returns(Questions);
            // When
            var quiz = await _quizService.GetQuizAsync(TestQuizId);
            // Then
            Assert.AreEqual(TestQuizId, quiz.Id);
            Assert.Greater(quiz.Date, DateTime.UtcNow.AddMilliseconds(-100));
            Assert.IsNull(quiz.Title);
            Assert.AreEqual(Questions, quiz.Questions);
        }
    }
}