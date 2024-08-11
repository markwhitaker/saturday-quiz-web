using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianQuizMetadataClient
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count);
}
