using SaturdayQuizWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public interface IQuizMetadataService
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count);
}

public class QuizMetadataService : IQuizMetadataService
{
    private readonly IDateTimeWrapper _dateTimeWrapper;
    private readonly IGuardianApiService _guardianApiService;
    private readonly IGuardianRssService _guardianRssService;

    public QuizMetadataService(
        IDateTimeWrapper dateTimeWrapper,
        IGuardianApiService guardianApiService,
        IGuardianRssService guardianRssService)
    {
        _dateTimeWrapper = dateTimeWrapper;
        _guardianApiService = guardianApiService;
        _guardianRssService = guardianRssService;
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        IReadOnlyList<QuizMetadata>? list = (await _guardianApiService.ListQuizzesAsync(count))?.Results
            .Select(item => new QuizMetadata
            {
                Id = item.Id,
                Title = item.WebTitle,
                Date = item.WebPublicationDate,
                Url = item.WebUrl
            })
            .OrderByDescending(qm => qm.Date)
            .ToList();

        if (list == null || !list.Any() || IsOlderThan1Week(list[0]))
        {
            list = await _guardianRssService.GetQuizMetadataAsync(count);
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
