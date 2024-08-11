using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.UnitTests.Models;

public class QuestionModelTests
{
    [Test]
    public void GivenNormalQuestion_WhenConstructed_ThenWhatLinksIsEmpty()
    {
        // Arrange
        var question = new QuestionModel
        {
            Number = 1,
            Type = QuestionType.Normal,
            Question = "What is the capital of France?"
        };

        // Act
        var actualWhatLinks = question.WhatLinks;

        // Assert
        Assert.That(actualWhatLinks, Is.Empty);
    }

    [Test]
    public void GivenWhatLinksQuestion_WhenConstructed_ThenWhatLinksIsPopulated()
    {
        // Arrange
        var question = new QuestionModel
        {
            Number = 1,
            Type = QuestionType.WhatLinks,
            Question = "red; green; blue?"
        };

        var expectedWhatLinks = new[] { "red", "green", "blue" };

        // Act
        var actualWhatLinks = question.WhatLinks;

        // Assert
        Assert.That(actualWhatLinks, Is.EquivalentTo(expectedWhatLinks));
    }
}
