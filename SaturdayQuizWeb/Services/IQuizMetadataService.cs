using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services;

public interface IQuizMetadataService
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count);
}