using System.Collections.Generic;
using System.Linq;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.Services
{
    public interface IHtmlService
    {
        IEnumerable<Question> FindQuestions(string html);
    }

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

        public IEnumerable<Question> FindQuestions(string html)
        {
            var rawSections = _sectionExtractor.ExtractSectionParagraphs(html)
                .Select(section => _htmlStripper.StripHtml(section));

            var splitSections = _sectionSplitter.SplitSections(rawSections);

            var questions = _questionAssembler.AssembleQuestions(splitSections);

            return questions;
        }
    }
}
