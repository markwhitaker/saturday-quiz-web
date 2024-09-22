namespace SaturdayQuizWeb.Models;

public sealed record Quiz
{
    public required string Id { get; init; } = string.Empty;

    public required DateTime Date { get; init; }

    public required string Title { get; init; } = string.Empty;

    public required IEnumerable<QuestionModel> Questions { get; init; } = new List<QuestionModel>();
}
