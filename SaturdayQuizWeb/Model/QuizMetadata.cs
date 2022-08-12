using System;

namespace SaturdayQuizWeb.Model;

public record struct QuizMetadata
{
    private readonly DateTime _date;

    public QuizMetadata()
    {
        _date = default;
    }

    public string Id { get; init; } = string.Empty;
    public DateTime Date
    {
        get => _date;
        init => _date = value.Date;
    }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
