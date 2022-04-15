using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.UnitTests.Services;

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

    private readonly QuizMetadata _quizMetadata = new()
    {
        Id = TestQuizId,
        Date = TestQuizDate,
        Title = TestQuizTitle,
        Url = TestQuizUrl
    };

    private readonly List<QuestionModel> _questions = new()
    {
        new QuestionModel
        {
            Number = TestQuestionNumber,
            Type = TestQuestionType,
            Question = TestQuestionText,
            Answer = TestQuestionAnswer
        }
    };

    // Mocks
    private IDateTimeWrapper _mockDateTimeWrapper = null!;
    private IGuardianScraperHttpService _mockScraperHttpService = null!;
    private IHtmlService _mockHtmlService = null!;
    private IQuizMetadataService _mockQuizMetadataService = null!;

    // Object under test
    private IQuizService _quizService = null!;

    [SetUp]
    public void Setup()
    {
        _mockDateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        _mockScraperHttpService = Substitute.For<IGuardianScraperHttpService>();
        _mockHtmlService = Substitute.For<IHtmlService>();
        _mockQuizMetadataService = Substitute.For<IQuizMetadataService>();
        _quizService = new QuizService(
            _mockDateTimeWrapper,
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
        Assert.That(quiz.Id, Is.EqualTo(TestQuizId));
        Assert.That(quiz.Date, Is.EqualTo(TestQuizDate));
        Assert.That(quiz.Title, Is.EqualTo(TestQuizTitle));
        Assert.That(quiz.Questions, Is.EqualTo(_questions));
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
        Assert.That(quiz.Id, Is.EqualTo(TestQuizId));
        Assert.That(quiz.Date, Is.EqualTo(TestQuizDate));
        Assert.That(quiz.Title, Is.EqualTo(TestQuizTitle));
        Assert.That(quiz.Questions, Is.EqualTo(_questions));
    }

    [Test]
    public async Task GivenScraperServiceReturnsContent_WhenGetQuizAsyncWithNonNullId_ThenExpectedQuizReturned()
    {
        // Given
        var expectedQuizDate = DateTime.UtcNow;
        _mockDateTimeWrapper.UtcNow.Returns(expectedQuizDate);
        _mockScraperHttpService.GetQuizPageContentAsync(TestQuizId).Returns(TestHtmlContent);
        _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);

        // When
        var quiz = await _quizService.GetQuizAsync(TestQuizId);

        // Then
        Assert.That(quiz.Id, Is.EqualTo(TestQuizId));
        Assert.That(quiz.Date, Is.EqualTo(expectedQuizDate));
        Assert.That(quiz.Title, Is.Empty);
        Assert.That(quiz.Questions, Is.EqualTo(_questions));
    }
}
