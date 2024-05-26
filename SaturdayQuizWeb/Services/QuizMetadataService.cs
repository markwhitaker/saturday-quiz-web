using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public class QuizMetadataService : IQuizMetadataService
{
    private readonly IDateTimeWrapper _dateTimeWrapper;
    private readonly IGuardianApiClient _guardianApiClient;
    private readonly IGuardianRssClient _guardianRssClient;
    private readonly ILogger<QuizMetadataService> _logger;

    public QuizMetadataService(
        IDateTimeWrapper dateTimeWrapper,
        IGuardianApiClient guardianApiClient,
        IGuardianRssClient guardianRssClient,
        ILogger<QuizMetadataService> logger)
    {
        _dateTimeWrapper = dateTimeWrapper;
        _guardianApiClient = guardianApiClient;
        _guardianRssClient = guardianRssClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var set = (await _guardianApiClient.GetQuizMetadataAsync(count)).ToHashSet();

        if (!IsUpToDate(set))
        {
            _logger.LogWarning("Didn't get up-to-date quiz metadata from API, Trying RSS...");
            set.UnionWith(await _guardianRssClient.GetQuizMetadataAsync(count));
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
        _dateTimeWrapper.UtcNow.Date.Subtract(quizMetadata.Date.Date) >= TimeSpan.FromDays(7);
}
