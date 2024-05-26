using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public class QuizMetadataService(
    IDateTimeWrapper dateTimeWrapper,
    IGuardianApiClient guardianApiClient,
    IGuardianRssClient guardianRssClient,
    ILogger<QuizMetadataService> logger)
    : IQuizMetadataService
{
    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var set = (await guardianApiClient.GetQuizMetadataAsync(count)).ToHashSet();

        if (!IsUpToDate(set))
        {
            logger.LogWarning("Didn't get up-to-date quiz metadata from API, Trying RSS...");
            set.UnionWith(await guardianRssClient.GetQuizMetadataAsync(count));
        }

        if (set.Count == 0)
        {
            throw new Exception("Couldn't get data from the Guardian API or RSS feed");
        }

        return set
            .OrderByDescending(qm => qm.Date)
            .Take(count)
            .ToList();
    }

    private bool IsUpToDate(IEnumerable<QuizMetadata> quizMetadataSet)
    {
        var newestItem = quizMetadataSet.MaxBy(qm => qm.Date);
        return newestItem != null && !IsOlderThanOneWeek(newestItem);
    }

    private bool IsOlderThanOneWeek(QuizMetadata quizMetadata) =>
        dateTimeWrapper.UtcNow.Date.Subtract(quizMetadata.Date.Date) >= TimeSpan.FromDays(7);
}
