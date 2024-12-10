using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Services;

public interface IQuizService
{
    Task<Quiz> GetLatestQuizAsync();
    Task<Quiz> GetQuizAsync(DateTime date);
    Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata);
}
