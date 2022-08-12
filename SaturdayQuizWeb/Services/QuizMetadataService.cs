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
        var list = await _guardianApiClient.GetQuizMetadataAsync(count);

        if (!list.Any() || IsOlderThan1Week(list[0]))
        {
            list = await _guardianRssClient.GetQuizMetadataAsync(count);
        }

        if (list == null)
        {
            throw new Exception("Couldn't get data from the Guardian API or RSS feed");
        }

        return list;
    }

    private bool IsOlderThan1Week(QuizMetadata quizMetadata) =>
        _dateTimeWrapper.UtcNow.Date.Subtract(quizMetadata.Date.Date) >= TimeSpan.FromDays(7);
}
