using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services.Parsing;

public interface IQuestionAssembler
{
    IEnumerable<QuestionModel> AssembleQuestions(IEnumerable<string> questionsSection, IEnumerable<string> answersSection);
}