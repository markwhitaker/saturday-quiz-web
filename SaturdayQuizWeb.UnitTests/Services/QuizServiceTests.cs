using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Models;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.UnitTests.Services;

[TestFixture]
[TestOf(typeof(QuizService))]
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

    private readonly List<QuestionModel> _questions =
    [
        new QuestionModel
        {
            Number = TestQuestionNumber,
            Type = TestQuestionType,
            Question = TestQuestionText,
            Answer = TestQuestionAnswer
        }
    ];

    // Mocks
    private IDateTimeWrapper _mockDateTimeWrapper = null!;
    private IGuardianWebsiteHttpClient _mockGuardianWebsiteHttpClient = null!;
    private IHtmlService _mockHtmlService = null!;
    private IQuizMetadataService _mockQuizMetadataService = null!;

    // Object under test
    private IQuizService _quizService = null!;

    [SetUp]
    public void Setup()
    {
        _mockDateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        _mockGuardianWebsiteHttpClient = Substitute.For<IGuardianWebsiteHttpClient>();
        _mockHtmlService = Substitute.For<IHtmlService>();
        _mockQuizMetadataService = Substitute.For<IQuizMetadataService>();
        _quizService = new QuizService(
            _mockDateTimeWrapper,
            _mockGuardianWebsiteHttpClient,
            _mockHtmlService,
            _mockQuizMetadataService);
    }

    [Test]
    public async Task GivenGuardianWebsiteServiceReturnsContent_WhenGetQuizAsyncByMetadata_ThenExpectedQuizReturned()
    {
        // Given
        _mockGuardianWebsiteHttpClient.GetStringAsync(TestQuizId).Returns(TestHtmlContent);
        _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);

        // When
        var quiz = await _quizService.GetQuizAsync(_quizMetadata);

        // Then
        Assert.That(quiz.Id, Is.EqualTo(TestQuizId));
        Assert.That(quiz.Date, Is.EqualTo(TestQuizDate.Date));
        Assert.That(quiz.Title, Is.EqualTo(TestQuizTitle));
        Assert.That(quiz.Questions, Is.EqualTo(_questions));
    }

    [Test]
    public async Task GivenGuardianWebsiteServiceReturnsContent_WhenGetQuizAsyncWithNullId_ThenExpectedQuizReturned()
    {
        // Given
        _mockQuizMetadataService.GetQuizMetadataAsync(1).Returns(new List<QuizMetadata>
        {
            _quizMetadata
        });
        _mockGuardianWebsiteHttpClient.GetStringAsync(TestQuizId).Returns(TestHtmlContent);
        _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);

        // When
        var quiz = await _quizService.GetQuizAsync();

        // Then
        Assert.That(quiz.Id, Is.EqualTo(TestQuizId));
        Assert.That(quiz.Date, Is.EqualTo(TestQuizDate.Date));
        Assert.That(quiz.Title, Is.EqualTo(TestQuizTitle));
        Assert.That(quiz.Questions, Is.EqualTo(_questions));
    }

    [Test]
    public async Task GivenGuardianWebsiteServiceReturnsContent_WhenGetQuizAsyncWithNonNullId_ThenExpectedQuizReturned()
    {
        // Given
        var expectedQuizDate = DateTime.UtcNow;
        _mockDateTimeWrapper.UtcNow.Returns(expectedQuizDate);
        _mockGuardianWebsiteHttpClient.GetStringAsync(TestQuizId).Returns(TestHtmlContent);
        _mockHtmlService.FindQuestions(TestHtmlContent).Returns(_questions);

        // When
        var quiz = await _quizService.GetQuizAsync(TestQuizId);

        // Then
        Assert.That(quiz.Id, Is.EqualTo(TestQuizId));
        Assert.That(quiz.Date, Is.EqualTo(expectedQuizDate.Date));
        Assert.That(quiz.Title, Is.Empty);
        Assert.That(quiz.Questions, Is.EqualTo(_questions));
    }
}
