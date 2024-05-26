using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.Services;

public class HtmlService : IHtmlService
{
    private readonly ISectionExtractor _sectionExtractor;
    private readonly IHtmlStripper _htmlStripper;
    private readonly ISectionSplitter _sectionSplitter;
    private readonly IQuestionAssembler _questionAssembler;

    public HtmlService(
        ISectionExtractor sectionExtractor,
        IHtmlStripper htmlStripper,
        ISectionSplitter sectionSplitter,
        IQuestionAssembler questionAssembler)
    {
        _sectionExtractor = sectionExtractor;
        _htmlStripper = htmlStripper;
        _sectionSplitter = sectionSplitter;
        _questionAssembler = questionAssembler;
    }

    public IEnumerable<QuestionModel> FindQuestions(string html)
    {
        var sections = _sectionExtractor.ExtractSections(html);

        var questionsSection = _htmlStripper.StripHtml(sections.QuestionsSectionHtml);
        var answersSection = _htmlStripper.StripHtml(sections.AnswersSectionHtml);

        var questionsSectionSplit = _sectionSplitter.SplitSection(questionsSection);
        var answersSectionSplit = _sectionSplitter.SplitSection(answersSection);

        var questions = _questionAssembler.AssembleQuestions(
            questionsSectionSplit,
            answersSectionSplit);

        return questions;
    }
}
