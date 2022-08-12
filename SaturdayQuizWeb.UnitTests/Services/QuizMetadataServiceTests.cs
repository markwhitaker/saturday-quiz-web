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
        var quizDate = today.Subtract(TimeSpan.FromDays(ageInDays));
        var expectedApiResponse = new List<QuizMetadata>
        {
            new()
            {
                Id = "id",
                Date = quizDate,
                Title = "title",
                Url = "url"
            }
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedApiResponse);

        // When
        await _quizMetadataService.GetQuizMetadataAsync(1);

        // Then
        await _mockGuardianApiClient.Received().GetQuizMetadataAsync(1);
        await _mockGuardianRssClient.DidNotReceiveWithAnyArgs().GetQuizMetadataAsync(default);
    }

    [TestCase(7)]
    [TestCase(8)]
    [TestCase(14)]
    [TestCase(9999)]
    public async Task GivenLatestQuizReturnedByApiIsAWeekOldOrOlder_WhenGettingMetadata_ThenRssIsUsed(int ageInDays)
    {
        // Given
        var today = new DateTime(2022, 1, 8, 23, 59, 59);
        var quizDate = today.Subtract(TimeSpan.FromDays(ageInDays));
        var expectedApiResponse = new List<QuizMetadata>
        {
            new()
            {
                Id = "id",
                Date = quizDate,
                Title = "title",
                Url = "url"
            }
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiClient.GetQuizMetadataAsync(default).ReturnsForAnyArgs(expectedApiResponse);

        // When
        await _quizMetadataService.GetQuizMetadataAsync(1);

        // Then
        await _mockGuardianApiClient.Received().GetQuizMetadataAsync(1);
        await _mockGuardianRssClient.Received().GetQuizMetadataAsync(1);
    }
}
