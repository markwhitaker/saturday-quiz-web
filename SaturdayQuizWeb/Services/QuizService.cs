using System;
using System.Linq;
using System.Threading.Tasks;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services;

public interface IQuizService
{
    Task<Quiz> GetQuizAsync(string? id = null);
    Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata);
}

public class QuizService : IQuizService
{
    private readonly IGuardianScraperHttpService _scraperHttpService;
    private readonly IHtmlService _htmlService;
    private readonly IQuizMetadataService _quizMetadataService;

    public QuizService(
        IGuardianScraperHttpService scraperHttpService,
        IHtmlService htmlService,
        IQuizMetadataService quizMetadataService)
    {
        _scraperHttpService = scraperHttpService;
        _htmlService = htmlService;
        _quizMetadataService = quizMetadataService;
    }

    public async Task<Quiz> GetQuizAsync(string? id)
    {
        QuizMetadata quizMetadata;

        if (id == null)
        {
            var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(1);
            quizMetadata = quizMetadataList.First();
        }
        else
        {
            quizMetadata = new QuizMetadata
            {
                Id = id,
                Date = DateTime.UtcNow
            };
        }

        return await GetQuizAsync(quizMetadata);
    }

    public async Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata)
    {
        var quizHtml = await _scraperHttpService.GetQuizPageContentAsync(quizMetadata.Id);
        var questions = _htmlService.FindQuestions(quizHtml);
        return new Quiz
        {
            Id = quizMetadata.Id,
            Date = quizMetadata.Date,
            Title = quizMetadata.Title,
            Questions = questions
        };
    }
}
