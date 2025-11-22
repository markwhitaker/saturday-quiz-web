using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Models;

namespace SaturdayQuizWeb.Services;

public interface IQuizService
{
    Task<Quiz> GetLatestQuizAsync();
    Task<Quiz> GetQuizAsync(DateTime date);
    Task<Quiz> GetQuizAsync(QuizMetadata quizMetadata);
}

public class QuizService(
    IGuardianWebsiteHttpClient guardianWebsiteHttpClient,
    IHtmlService htmlService,
    IQuizMetadataService quizMetadataService)
    : IQuizService
{
    public async Task<Quiz> GetLatestQuizAsync()
    {
        var quizMetadataList = await quizMetadataService.GetQuizMetadataAsync(1);
        var quizMetadata = quizMetadataList[0];
        return await GetQuizAsync(quizMetadata);
    }

    public async Task<Quiz> GetQuizAsync(DateTime date)
    {
        var quizMetadataList = await quizMetadataService.GetQuizMetadataAsync(50);
        var quizMetadata = quizMetadataList.FirstOrDefault(qm => qm.Date.Date == date.Date);
        if (quizMetadata == null)
        {
            throw new Exception($"Quiz not found for date {date:yyyy-MM-dd}");
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