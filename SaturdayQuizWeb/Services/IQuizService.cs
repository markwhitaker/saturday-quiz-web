using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Services;

public interface IQuizService
{
    Task<Quiz> GetQuizAsync(string? id = null);
    Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata);
}