using System;

namespace SaturdayQuizWeb.Model;

public record QuizMetadata
{
    public string Id { get; init; }
    public DateTime Date { get; init; }
    public string Title { get; init; }
    public string Url { get; init; }
}