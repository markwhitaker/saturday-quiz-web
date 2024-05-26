using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb.Services;

public class QuizService(
    IDateTimeWrapper dateTimeWrapper,
    IGuardianWebsiteHttpClient guardianWebsiteHttpClient,
    IHtmlService htmlService,
    IQuizMetadataService quizMetadataService)
    : IQuizService
{
    public async Task<Quiz> GetQuizAsync(string? id)
    {
        QuizMetadata quizMetadata;

        if (id == null)
        {
            var quizMetadataList = await quizMetadataService.GetQuizMetadataAsync(1);
            quizMetadata = quizMetadataList[0];
        }
        else
        {
            quizMetadata = new QuizMetadata
            {
                Id = id,
                Date = dateTimeWrapper.UtcNow
            };
        }

        return await GetQuizAsync(quizMetadata);
    }

    public async Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata)
    {
        var quizHtml = await guardianWebsiteHttpClient.GetStringAsync(quizMetadata.Id);
        var questions = htmlService.FindQuestions(quizHtml);
        return new Quiz
        {
            Id = quizMetadata.Id,
            Date = quizMetadata.Date,
            Title = quizMetadata.Title,
            Questions = questions
        };
    }
}
