namespace SaturdayQuizWeb.Model.Parsing;

public sealed record Sections
{
    public string QuestionsSectionHtml { get; init; } = string.Empty;

    public string AnswersSectionHtml { get; init; } = string.Empty;
}
