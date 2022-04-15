using System;
using System.Collections.Generic;

namespace SaturdayQuizWeb.Model;

public record Quiz
{
    public string Id { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string Title { get; init; } = string.Empty;
    public IEnumerable<QuestionModel> Questions { get; init; } = new List<QuestionModel>();
}
