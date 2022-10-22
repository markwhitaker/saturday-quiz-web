using System;
using NUnit.Framework;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.UnitTests.Models;

[TestFixture]
public class QuizMetadataTests
{
    [TestCase(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Local)]
    [TestCase(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified)]
    [TestCase(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc)]
    [TestCase(2022, 12, 31, 12, 34, 56, 789, DateTimeKind.Local)]
    [TestCase(2022, 12, 31, 12, 34, 56, 789, DateTimeKind.Unspecified)]
    [TestCase(2022, 12, 31, 12, 34, 56, 789, DateTimeKind.Utc)]
    [TestCase(2022, 12, 31, 23, 59, 59, 999, DateTimeKind.Local)]
    [TestCase(2022, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified)]
    [TestCase(2022, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)]
    public void GivenDateTime_WhenQuizMetadataIsInitialised_ThenExpectedDateIsSet(
        int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
    {
        // Given
        var expectedDate = new DateTime(2022, 12, 31, 0, 0, 0, DateTimeKind.Unspecified);
        var inputDateTime = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

        // When
        var quizMetadata = new QuizMetadata
        {
            Date = inputDateTime
        };

        // Then
        Assert.That(quizMetadata.Date, Is.EqualTo(expectedDate));
    }

    [Test]
    public void GivenTwoQuizMetadatasWithSameUrl_WhenCompared_ThenMetadatasAreEqual()
    {
        // Given
        const string commonUrl = "expected-url";
        var quizMetadata1 = new QuizMetadata
        {
            Id = "id-1",
            Title = "title-1",
            Date = new DateTime(2022, 1, 2),
            Url = commonUrl,
            Source = "source-1"
        };
        var quizMetadata2 = new QuizMetadata
        {
            Id = "id-2",
            Title = "title-2",
            Date = new DateTime(2022, 3, 4),
            Url = commonUrl,
            Source = "source-2"
        };

        // When/Then
        Assert.That(quizMetadata1, Is.EqualTo(quizMetadata2));
    }

    [Test]
    public void GivenTwoQuizMetadatasWithDifferentUrls_WhenCompared_ThenMetadatasAreNotEqual()
    {
        // Given
        const string commonId = "id";
        const string commonTitle = "title";
        const string commonSource = "source";
        var commonDate = DateTime.UtcNow;

        var quizMetadata1 = new QuizMetadata
        {
            Id = commonId,
            Title = commonTitle,
            Date = commonDate,
            Url = "url-1",
            Source = commonSource
        };
        var quizMetadata2 = new QuizMetadata
        {
            Id = commonId,
            Title = commonTitle,
            Date = commonDate,
            Url = "url-2",
            Source = commonSource
        };

        // When/Then
        Assert.That(quizMetadata1, Is.Not.EqualTo(quizMetadata2));
    }
}
