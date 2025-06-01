using JetBrains.Annotations;
using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Dtos;

public sealed record QuizDto
{
    public string Id { [UsedImplicitly] get; }
    public DateTime Date { [UsedImplicitly] get; }
    public string Title { [UsedImplicitly] get; }
    public QuestionDto[] Questions { [UsedImplicitly] get; }

    public QuizDto(Quiz quiz)
    {
        Id = quiz.Id;
        Date = quiz.Date;
        Title = quiz.Title;
        Questions = quiz.Questions.Select(q => new QuestionDto(q)).ToArray();
    }
}
