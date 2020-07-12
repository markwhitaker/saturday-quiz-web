using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Model.Parsing;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.UnitTests.Services
{
    [TestFixture]
    public class HtmlServiceTests
    {
        private ISectionExtractor _mockSectionExtractor;
        private IHtmlStripper _mockHtmlStripper;
        private ISectionSplitter _mockSectionSplitter;
        private IQuestionAssembler _mockQuestionAssember;
        
        private IHtmlService _htmlService;

        [SetUp]
        public void SetUp()
        {
            _mockSectionExtractor = Substitute.For<ISectionExtractor>();
            _mockHtmlStripper = Substitute.For<IHtmlStripper>();
            _mockSectionSplitter = Substitute.For<ISectionSplitter>();
            _mockQuestionAssember = Substitute.For<IQuestionAssembler>();
            
            _htmlService = new HtmlService(
                _mockSectionExtractor,
                _mockHtmlStripper,
                _mockSectionSplitter,
                _mockQuestionAssember);
        }

        [Test]
        public void GivenHtml_WhenFindQuestions_ThenExpectedQuestionsAreReturned()
        {
            // Given
            const string html = "html";
            const string expectedQuestionsHtml = "questionsHtml";
            const string expectedAnswersHtml = "answersHtml";
            const string expectedQuestionsHtmlStripped = "questionsHtmlStripped";
            const string expectedAnswersHtmlStripped = "answersHtmlStripped";
            var expectedSections = new Sections
            {
                QuestionsSectionHtml = expectedQuestionsHtml,
                AnswersSectionHtml = expectedAnswersHtml
            };
            var expectedQuestionsList = new List<string> {"questions"};
            var expectedAnswersList = new List<string> {"answers"};
            var expectedQuestions = new List<Question>
            {
                new Question
                {
                    Number = 1,
                    Type = QuestionType.Normal,
                    QuestionHtml = "questionHtml",
                    QuestionText = "questionText",
                    AnswerHtml = "answerHtml",
                    AnswerText = "answerText"
                }
            };

            _mockSectionExtractor.ExtractSections(html).Returns(expectedSections);
            _mockHtmlStripper.StripHtml(expectedQuestionsHtml).Returns(expectedQuestionsHtmlStripped);
            _mockHtmlStripper.StripHtml(expectedAnswersHtml).Returns(expectedAnswersHtmlStripped);
            _mockSectionSplitter.SplitSection(expectedQuestionsHtmlStripped).Returns(expectedQuestionsList);
            _mockSectionSplitter.SplitSection(expectedAnswersHtmlStripped).Returns(expectedAnswersList);
            _mockQuestionAssember.AssembleQuestions(expectedQuestionsList, expectedAnswersList).Returns(expectedQuestions);
            
            // When
            var actualQuestions = _htmlService.FindQuestions(html);
            
            // Then
            Assert.That(actualQuestions, Is.EqualTo(expectedQuestions));
        }
    }
}