using SaturdayQuizWeb.Models;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.Services;

public interface IHtmlService
{
    IEnumerable<QuestionModel> FindQuestions(string html);
}

public class HtmlService(
    ISectionExtractor sectionExtractor,
    IHtmlStripper htmlStripper,
    ISectionSplitter sectionSplitter,
    IQuestionAssembler questionAssembler)
    : IHtmlService
{
    public IEnumerable<QuestionModel> FindQuestions(string html)
    {
        var sections = sectionExtractor.ExtractQuestionsAndAnswersSections(html)
            .Select(htmlStripper.RemoveUnwantedHtmlTagsAndSpaces)
            .Select(sectionSplitter.SplitSectionIntoLines)
            .ToArray();

        var questions = questionAssembler.AssembleQuestions(
            sections.First(),
            sections.Last());

        return questions;
    }
}
