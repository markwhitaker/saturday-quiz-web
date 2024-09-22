namespace SaturdayQuizWeb.Models.Parsing;

public sealed record Sections
{
    public required string QuestionsSectionHtml { get; init; } = string.Empty;

    public required string AnswersSectionHtml { get; init; } = string.Empty;
}
