using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.UnitTests.Services;

[TestFixture]
public class QuizMetadataServiceTests
{
    private IDateTimeWrapper _mockDateTimeWrapper = null!;
    private IGuardianApiClient _mockGuardianApiClient = null!;
    private IGuardianRssClient _mockGuardianRssClient = null!;

    private IQuizMetadataService _quizMetadataService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockDateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        _mockGuardianApiClient = Substitute.For<IGuardianApiClient>();
        _mockGuardianRssClient = Substitute.For<IGuardianRssClient>();

        _quizMetadataService = new QuizMetadataService(
            _mockDateTimeWrapper,
            _mockGuardianApiClient,
            _mockGuardianRssClient);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(6)]
    public async Task GivenLatestQuizReturnedByApiIsLessThanAWeekOld_WhenGettingMetadata_ThenRssIsNotUsed(int ageInDays)
    {
        // Given
        var today = new DateTime(2022, 1, 8, 23, 59, 59);
        var oldDate = today.Subtract(TimeSpan.FromDays(ageInDays));
        var oldQuizMetadata = new QuizMetadata
        {
            Id = "id",
            Date = oldDate,
            Title = "title",
            Url = "url-1",
            Source = "API"
        };
        var newQuizMetadata = new QuizMetadata
        {
            Id = "id",
            Date = today,
            Title = "title",
            Url = "url-2",
            Source = "RSS"
        };
        var expectedApiResponse = new List<QuizMetadata>
        {
            newQuizMetadata,
            oldQuizMetadata
        };
        var expectedRssResponse = new List<QuizMetadata>
        {
            newQuizMetadata,
            oldQuizMetadata
        };
        var expectedMetadataServiceResponse = new List<QuizMetadata>
        {
            newQuizMetadata, oldQuizMetadata
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedApiResponse);
        _mockGuardianRssClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedRssResponse);

        // When
        var actualMetadataServiceResponse = await _quizMetadataService.GetQuizMetadataAsync(2);

        // Then
        await _mockGuardianApiClient.Received().GetQuizMetadataAsync(2);
        await _mockGuardianRssClient.DidNotReceiveWithAnyArgs().GetQuizMetadataAsync(default);
        Assert.That(expectedMetadataServiceResponse, Is.EqualTo(actualMetadataServiceResponse));
    }

    [TestCase(7)]
    [TestCase(8)]
    [TestCase(14)]
    [TestCase(9999)]
    public async Task GivenLatestQuizReturnedByApiIsAWeekOldOrOlder_WhenGettingMetadata_ThenRssIsUsed(int ageInDays)
    {
        // Given
        var today = new DateTime(2022, 1, 8, 23, 59, 59);
        var oldDate = today.Subtract(TimeSpan.FromDays(ageInDays));
        var oldQuizMetadata = new QuizMetadata
        {
            Id = "id",
            Date = oldDate,
            Title = "title",
            Url = "url-1",
            Source = "API"
        };
        var newQuizMetadata = new QuizMetadata
        {
            Id = "id",
            Date = today,
            Title = "title",
            Url = "url-2",
            Source = "RSS"
        };
        var expectedApiResponse = new List<QuizMetadata>
        {
            oldQuizMetadata
        };
        var expectedRssResponse = new List<QuizMetadata>
        {
            newQuizMetadata,
            oldQuizMetadata
        };
        var expectedMetadataServiceResponse = new List<QuizMetadata>
        {
            newQuizMetadata, oldQuizMetadata
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedApiResponse);
        _mockGuardianRssClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedRssResponse);

        // When
        var actualMetadataServiceResponse = await _quizMetadataService.GetQuizMetadataAsync(2);

        // Then
        await _mockGuardianApiClient.Received().GetQuizMetadataAsync(2);
        await _mockGuardianRssClient.Received().GetQuizMetadataAsync(2);
        Assert.That(expectedMetadataServiceResponse, Is.EqualTo(actualMetadataServiceResponse));
    }
}
