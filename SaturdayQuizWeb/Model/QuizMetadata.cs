using System;

namespace SaturdayQuizWeb.Model;

public record QuizMetadata
{
    public string Id { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
