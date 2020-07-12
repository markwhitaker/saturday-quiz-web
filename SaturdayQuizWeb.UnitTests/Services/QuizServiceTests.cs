using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.UnitTests.Services
{
    [TestFixture]
    public class QuizServiceTests
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
        
        private readonly QuizMetadata _quizMetadata = new QuizMetadata
        {
            Id = TestQuizId,
            Date = TestQuizDate,
            Title = TestQuizTitle,
            Url = TestQuizUrl
        };
        private readonly List<Question> _questions = new List<Question>
        {
            new Question
            {
                Number = TestQuestionNumber,
                Type = TestQuestionType,
                QuestionText = TestQuestionText,
                AnswerText = TestQuestionAnswer
            }
        };

        // Mocks
        private readonly IGuardianScraperHttpService _mockScraperHttpService;
        private readonly IHtmlService _mockHtmlService;
        private readonly IQuizMetadataService _mockQuizMetadataService;
        
        // Object under test
        private readonly IQuizService _quizService;

        public QuizServiceTests()
        {
            _mockScraperHttpService = Substitute.For<IGuardianScraperHttpService>();
            _mockHtmlService = Substitute.For<IHtmlService>();
            _mockQuizMetadataService = Substitute.For<IQuizMetadataService>();
            _quizService = new QuizService(
                _mockScraperHttpService,
                _mockHtmlService,
                _mockQuizMetadataService);
        }

        [Test]
        public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncByMetadata_ThenExpectedQuizReturned()
        {
            // Given
            _mockScraperHttpService.GetQuizPageContentAsync(TestQuizId).Returns(TestHtmlContent);
            _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);
            // When
            var quiz = await _quizService.GetQuizAsync(_quizMetadata);
            // Then
            Assert.AreEqual(TestQuizId, quiz.Id);
            Assert.AreEqual(TestQuizDate, quiz.Date);
            Assert.AreEqual(TestQuizTitle, quiz.Title);
            Assert.AreEqual(_questions, quiz.Questions);
        }
        
        [Test]
        public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncWithNullId_ThenExpectedQuizReturned()
        {
            // Given
            _mockQuizMetadataService.GetQuizMetadataAsync(1).Returns(new List<QuizMetadata>
            {
                _quizMetadata
            });
            _mockScraperHttpService.GetQuizPageContentAsync(TestQuizId).Returns(TestHtmlContent);
            _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);
            // When
            var quiz = await _quizService.GetQuizAsync();
            // Then
            Assert.AreEqual(TestQuizId, quiz.Id);
            Assert.AreEqual(TestQuizDate, quiz.Date);
            Assert.AreEqual(TestQuizTitle, quiz.Title);
            Assert.AreEqual(_questions, quiz.Questions);
        }
        
        [Test]
        public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncWithNonNullId_ThenExpectedQuizReturned()
        {
            // Given
            _mockScraperHttpService.GetQuizPageContentAsync(TestQuizId).Returns(TestHtmlContent);
            _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);
            // When
            var quiz = await _quizService.GetQuizAsync(TestQuizId);
            // Then
            Assert.AreEqual(TestQuizId, quiz.Id);
            Assert.Greater(quiz.Date, DateTime.UtcNow.AddMilliseconds(-100));
            Assert.IsNull(quiz.Title);
            Assert.AreEqual(_questions, quiz.Questions);
        }
    }
}