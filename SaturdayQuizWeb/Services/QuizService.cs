using System.Linq;
using System.Threading.Tasks;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public interface IQuizService
{
    Task<Quiz> GetQuizAsync(string? id = null);
    Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata);
}

public class QuizService : IQuizService
{
    private readonly IDateTimeWrapper _dateTimeWrapper;
    private readonly IGuardianWebsiteClient _guardianWebsiteClient;
    private readonly IHtmlService _htmlService;
    private readonly IQuizMetadataService _quizMetadataService;

    public QuizService(
        IDateTimeWrapper dateTimeWrapper,
        IGuardianWebsiteClient guardianWebsiteClient,
        IHtmlService htmlService,
        IQuizMetadataService quizMetadataService)
    {
        _dateTimeWrapper = dateTimeWrapper;
        _guardianWebsiteClient = guardianWebsiteClient;
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
                Date = _dateTimeWrapper.UtcNow
            };
        }

        return await GetQuizAsync(quizMetadata);
    }

    public async Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata)
    {
        var quizHtml = await _guardianWebsiteClient.GetPageContentAsync(quizMetadata.Id);
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
