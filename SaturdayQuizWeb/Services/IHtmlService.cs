using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services;

public interface IHtmlService
{
    IEnumerable<QuestionModel> FindQuestions(string html);
}