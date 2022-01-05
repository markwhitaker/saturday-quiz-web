using SaturdayQuizWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaturdayQuizWeb.Services;

public interface IQuizMetadataService
{
    Task<List<QuizMetadata>> GetQuizMetadataAsync(int count);
}

public class QuizMetadataService : IQuizMetadataService
{
    private readonly IGuardianApiHttpService _guardianApiHttpService;

    public QuizMetadataService(IGuardianApiHttpService guardianApiHttpService)
    {
        _guardianApiHttpService = guardianApiHttpService;
    }

    public async Task<List<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var response = await _guardianApiHttpService.ListQuizzesAsync(count);

        if (response == null)
        {
            throw new Exception("Something went wrong with the Guardian API call");
        }

        return response.Results
            .Select(item => new QuizMetadata
            {
                Id = item.Id,
                Title = item.WebTitle,
                Date = item.WebPublicationDate,
                Url = item.WebUrl
            })
            .ToList();
    }
}