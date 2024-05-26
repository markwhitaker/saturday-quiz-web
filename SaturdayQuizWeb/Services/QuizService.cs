using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public class QuizService : IQuizService
{
    private readonly IDateTimeWrapper _dateTimeWrapper;
    private readonly IGuardianWebsiteHttpClient _guardianWebsiteHttpClient;
    private readonly IHtmlService _htmlService;
    private readonly IQuizMetadataService _quizMetadataService;

    public QuizService(
        IDateTimeWrapper dateTimeWrapper,
        IGuardianWebsiteHttpClient guardianWebsiteHttpClient,
        IHtmlService htmlService,
        IQuizMetadataService quizMetadataService)
    {
        _dateTimeWrapper = dateTimeWrapper;
        _guardianWebsiteHttpClient = guardianWebsiteHttpClient;
        _htmlService = htmlService;
        _quizMetadataService = quizMetadataService;
    }

    public async Task<Quiz> GetQuizAsync(string? id)
    {
        QuizMetadata quizMetadata;

        if (id == null)
        {
            var quizMetadataList = await _quizMetadataService.GetQuizMetadataAsync(1);
            quizMetadata = quizMetadataList[0];
        }
        else
        {
            quizMetadata = new QuizMetadata
            {
                Id = id,
                Date = _dateTimeWrapper.UtcNow
            };
        }

        return await GetQuizAsync(quizMetadata);
    }

    public async Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata)
    {
        var quizHtml = await _guardianWebsiteHttpClient.GetStringAsync(quizMetadata.Id);
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
