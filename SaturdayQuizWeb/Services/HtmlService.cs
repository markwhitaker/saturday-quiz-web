using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.Services;

public class HtmlService(
    ISectionExtractor sectionExtractor,
    IHtmlStripper htmlStripper,
    ISectionSplitter sectionSplitter,
    IQuestionAssembler questionAssembler)
    : IHtmlService
{
    public IEnumerable<QuestionModel> FindQuestions(string html)
    {
        var sections = sectionExtractor.ExtractSections(html);

        var questionsSection = htmlStripper.StripHtml(sections.QuestionsSectionHtml);
        var answersSection = htmlStripper.StripHtml(sections.AnswersSectionHtml);

        var questionsSectionSplit = sectionSplitter.SplitSection(questionsSection);
        var answersSectionSplit = sectionSplitter.SplitSection(answersSection);

        var questions = questionAssembler.AssembleQuestions(
            questionsSectionSplit,
            answersSectionSplit);

        return questions;
    }
}
