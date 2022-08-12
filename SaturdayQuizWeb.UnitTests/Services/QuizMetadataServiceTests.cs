using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SaturdayQuizWeb.Model.Api;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.UnitTests.Services;

[TestFixture]
public class QuizMetadataServiceTests
{
    private IDateTimeWrapper _mockDateTimeWrapper = null!;
    private IGuardianApiService _mockGuardianApiService = null!;
    private IGuardianRssService _mockGuardianRssService = null!;

    private IQuizMetadataService _quizMetadataService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockDateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        _mockGuardianApiService = Substitute.For<IGuardianApiService>();
        _mockGuardianRssService = Substitute.For<IGuardianRssService>();

        _quizMetadataService = new QuizMetadataService(
            _mockDateTimeWrapper,
            _mockGuardianApiService,
            _mockGuardianRssService);
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
        var expectedApiResponse = new GuardianApiResponse
        {
            Response = new GuardianApiResponse.ResponseBody
            {
                Results = new List<GuardianApiQuizSummary>
                {
                    new GuardianApiQuizSummary
                    {
                        Id = "id",
                        WebPublicationDate = quizDate,
                        WebTitle = "web-title",
                        WebUrl = "web-url"
                    }
                }
            }
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiService.ListQuizzesAsync(default).ReturnsForAnyArgs(expectedApiResponse);

        // When
        await _quizMetadataService.GetQuizMetadataAsync(1);

        // Then
        await _mockGuardianApiService.Received().ListQuizzesAsync(1);
        await _mockGuardianRssService.DidNotReceiveWithAnyArgs().GetQuizMetadataAsync(default);
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
        var expectedApiResponse = new GuardianApiResponse
        {
            Response = new GuardianApiResponse.ResponseBody
            {
                Results = new List<GuardianApiQuizSummary>
                {
                    new GuardianApiQuizSummary
                    {
                        Id = "id",
                        WebPublicationDate = quizDate,
                        WebTitle = "web-title",
                        WebUrl = "web-url"
                    }
                }
            }
        };

        _mockDateTimeWrapper.UtcNow.Returns(today);
        _mockGuardianApiService.ListQuizzesAsync(default).ReturnsForAnyArgs(expectedApiResponse);

        // When
        await _quizMetadataService.GetQuizMetadataAsync(1);

        // Then
        await _mockGuardianApiService.Received().ListQuizzesAsync(1);
        await _mockGuardianRssService.Received().GetQuizMetadataAsync(1);
    }
}
