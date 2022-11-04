using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianQuizMetadataClient
{
    Task<IReadOnlySet<QuizMetadata>> GetQuizMetadataAsync(int count);
}
