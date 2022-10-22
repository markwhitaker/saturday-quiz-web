using SaturdayQuizWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public interface IQuizMetadataService
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count);
}

public class QuizMetadataService : IQuizMetadataService
{
    private readonly IDateTimeWrapper _dateTimeWrapper;
    private readonly IGuardianApiClient _guardianApiClient;
    private readonly IGuardianRssClient _guardianRssClient;

    public QuizMetadataService(
        IDateTimeWrapper dateTimeWrapper,
        IGuardianApiClient guardianApiClient,
        IGuardianRssClient guardianRssClient)
    {
        _dateTimeWrapper = dateTimeWrapper;
        _guardianApiClient = guardianApiClient;
        _guardianRssClient = guardianRssClient;
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var set = (await _guardianApiClient.GetQuizMetadataAsync(count)).ToHashSet();

        if (!IsUpToDate(set))
        {
            set.UnionWith(await _guardianRssClient.GetQuizMetadataAsync(count));
        }

        if (!set.Any())
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
        return newestItem != null && !IsOlderThan1Week(newestItem);
    }

    private bool IsOlderThan1Week(QuizMetadata quizMetadata) =>
        _dateTimeWrapper.UtcNow.Date.Subtract(quizMetadata.Date.Date) >= TimeSpan.FromDays(7);
}
