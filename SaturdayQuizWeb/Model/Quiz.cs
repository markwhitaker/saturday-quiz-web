using System;
using System.Collections.Generic;

namespace SaturdayQuizWeb.Model;

public record Quiz
{
    public string Id { get; init; }
    public DateTime Date { get; init; }
    public string Title { get; init; }
    public IEnumerable<QuestionModel> Questions { get; init; }
}