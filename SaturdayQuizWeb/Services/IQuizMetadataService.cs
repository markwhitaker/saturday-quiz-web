using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Services;

public interface IQuizMetadataService
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count);
}