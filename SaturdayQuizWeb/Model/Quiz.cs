﻿namespace SaturdayQuizWeb.Model;

public sealed record Quiz
{
    public string Id { get; init; } = string.Empty;

    public DateTime Date { get; init; }

    public string Title { get; init; } = string.Empty;

    public IEnumerable<QuestionModel> Questions { get; init; } = new List<QuestionModel>();
}
