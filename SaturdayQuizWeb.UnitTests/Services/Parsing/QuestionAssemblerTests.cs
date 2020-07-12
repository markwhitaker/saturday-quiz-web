using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.UnitTests.Services.Parsing
{
    [TestFixture]
    public class QuestionAssemblerTests
    {
        private readonly IQuestionAssembler _questionAssembler = new QuestionAssembler();
        private IList<Question> _questions;

        private static readonly IEnumerable<string> QuestionsSection = new List<string>
        {
            "1 Which Nazi leader died in Paddington in 1981?",
            "2 What are produced at <i>La Masia</i> &amp; <i>La Fábrica</i>?",
            "3 In publishing, what does ISBN stand for?",
            "4 Adopted in 1625, what symbol is the Dannebrog?",
            "5 Gabriele Münter was a founder member of what expressionist group?",
            "6 What was nicknamed the Honourable John Company?",
            "7 Which country separates Guyana and French Guiana?",
            "8 In what novel is Constance unhappily married to Sir Clifford?",
            "What links:",
            "9 Asgard and Midgard, in the form of a rainbow?",
            "10 Singer O’Dowd; outlaw McCarty; slugger Ruth; bank robber Nelson?",
            "11 Statant; sejant; rampant; passant; dormant?",
            "12 Victoria Embankment; Cardiff City Hall; Colchester station?",
            "13 Khumbu icefall; Kangshung face; Hornbein couloir; Hillary step?",
            "14 Prayers at 6am; 0 degrees longitude; 2, 3, 5, 7, etc?",
            "15 Prince of Morocco (Au); Prince of Arragon (Ag); Bassanio (Pb)?"
        };

        private static readonly IEnumerable<string> AnswersSection = new List<string>
        {
            "1 Albert Speer.",
            "2 Footballers (academies of <i>Barcelona</i> and <i>Real Madrid</i>).",
            "3 International Standard Book Number.",
            "4 Danish flag.",
            "5 Der Blaue Reiter (Blue Rider).",
            "6 East India Company.",
            "7 Suriname.",
            "8 Lady Chatterley’s Lover.",
            "9 Bifrost (bridge in Norse myth, linking gods’ realm and Earth).",
            "10 Young nicknames: Boy George; Billy the Kid; Babe Ruth; Baby Face Nelson.",
            "11 Attitudes of animals in heraldry: standing; sitting; rearing; walking; lying down.",
            "12 Statues of Boudicca.",
            "13 Parts of Mount Everest.",
            "14 Prime: canonical hour of prayer; prime meridian; prime numbers.",
            "15 Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead."
        };

        [SetUp]
        public void SetUp()
        {
            _questions = _questionAssembler.AssembleQuestions(QuestionsSection, AnswersSection).ToList();
        }

        [Test]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenQuestionCountIs15()
        {
            Assert.That(_questions.Count, Is.EqualTo(15));
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 5)]
        [TestCase(5, 6)]
        [TestCase(6, 7)]
        [TestCase(7, 8)]
        [TestCase(8, 9)]
        [TestCase(9, 10)]
        [TestCase(10, 11)]
        [TestCase(11, 12)]
        [TestCase(12, 13)]
        [TestCase(13, 14)]
        [TestCase(14, 15)]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenQuestionNumberIsCorrect(int questionIndex, int expectedQuestionNumber)
        {
            Assert.That(_questions[questionIndex].Number, Is.EqualTo(expectedQuestionNumber));
        }

        [TestCase(0, QuestionType.Normal)]
        [TestCase(1, QuestionType.Normal)]
        [TestCase(2, QuestionType.Normal)]
        [TestCase(3, QuestionType.Normal)]
        [TestCase(4, QuestionType.Normal)]
        [TestCase(5, QuestionType.Normal)]
        [TestCase(6, QuestionType.Normal)]
        [TestCase(7, QuestionType.Normal)]
        [TestCase(8, QuestionType.WhatLinks)]
        [TestCase(9, QuestionType.WhatLinks)]
        [TestCase(10, QuestionType.WhatLinks)]
        [TestCase(11, QuestionType.WhatLinks)]
        [TestCase(12, QuestionType.WhatLinks)]
        [TestCase(13, QuestionType.WhatLinks)]
        [TestCase(14, QuestionType.WhatLinks)]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenQuestionTypeIsCorrect(int questionIndex, QuestionType expectedQuestionType)
        {
            Assert.That(_questions[questionIndex].Type, Is.EqualTo(expectedQuestionType));
        }

        [TestCase(0, "Which Nazi leader died in Paddington in 1981?")]
        [TestCase(1, "What are produced at La Masia & La Fábrica?")]
        [TestCase(2, "In publishing, what does ISBN stand for?")]
        [TestCase(3, "Adopted in 1625, what symbol is the Dannebrog?")]
        [TestCase(4, "Gabriele Münter was a founder member of what expressionist group?")]
        [TestCase(5, "What was nicknamed the Honourable John Company?")]
        [TestCase(6, "Which country separates Guyana and French Guiana?")]
        [TestCase(7, "In what novel is Constance unhappily married to Sir Clifford?")]
        [TestCase(8, "Asgard and Midgard, in the form of a rainbow?")]
        [TestCase(9, "Singer O’Dowd; outlaw McCarty; slugger Ruth; bank robber Nelson?")]
        [TestCase(10, "Statant; sejant; rampant; passant; dormant?")]
        [TestCase(11, "Victoria Embankment; Cardiff City Hall; Colchester station?")]
        [TestCase(12, "Khumbu icefall; Kangshung face; Hornbein couloir; Hillary step?")]
        [TestCase(13, "Prayers at 6am; 0 degrees longitude; 2, 3, 5, 7, etc?")]
        [TestCase(14, "Prince of Morocco (Au); Prince of Arragon (Ag); Bassanio (Pb)?")]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenQuestionTextIsCorrect(int questionIndex, string expectedQuestionText)
        {
            Assert.That(_questions[questionIndex].QuestionText, Is.EqualTo(expectedQuestionText));
        }

        [TestCase(0, "Which Nazi leader died in Paddington in 1981?")]
        [TestCase(1, "What are produced at <i>La Masia</i> &amp; <i>La Fábrica</i>?")]
        [TestCase(2, "In publishing, what does ISBN stand for?")]
        [TestCase(3, "Adopted in 1625, what symbol is the Dannebrog?")]
        [TestCase(4, "Gabriele Münter was a founder member of what expressionist group?")]
        [TestCase(5, "What was nicknamed the Honourable John Company?")]
        [TestCase(6, "Which country separates Guyana and French Guiana?")]
        [TestCase(7, "In what novel is Constance unhappily married to Sir Clifford?")]
        [TestCase(8, "Asgard and Midgard, in the form of a rainbow?")]
        [TestCase(9, "Singer O’Dowd; outlaw McCarty; slugger Ruth; bank robber Nelson?")]
        [TestCase(10, "Statant; sejant; rampant; passant; dormant?")]
        [TestCase(11, "Victoria Embankment; Cardiff City Hall; Colchester station?")]
        [TestCase(12, "Khumbu icefall; Kangshung face; Hornbein couloir; Hillary step?")]
        [TestCase(13, "Prayers at 6am; 0 degrees longitude; 2, 3, 5, 7, etc?")]
        [TestCase(14, "Prince of Morocco (Au); Prince of Arragon (Ag); Bassanio (Pb)?")]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenQuestionHtmlIsCorrect(int questionIndex, string expectedQuestionHtml)
        {
            Assert.That(_questions[questionIndex].QuestionHtml, Is.EqualTo(expectedQuestionHtml));
        }

        [TestCase(0, "Albert Speer")]
        [TestCase(1, "Footballers (academies of Barcelona and Real Madrid)")]
        [TestCase(2, "International Standard Book Number")]
        [TestCase(3, "Danish flag")]
        [TestCase(4, "Der Blaue Reiter (Blue Rider)")]
        [TestCase(5, "East India Company")]
        [TestCase(6, "Suriname")]
        [TestCase(7, "Lady Chatterley’s Lover")]
        [TestCase(8, "Bifrost (bridge in Norse myth, linking gods’ realm and Earth)")]
        [TestCase(9, "Young nicknames: Boy George; Billy the Kid; Babe Ruth; Baby Face Nelson")]
        [TestCase(10, "Attitudes of animals in heraldry: standing; sitting; rearing; walking; lying down")]
        [TestCase(11, "Statues of Boudicca")]
        [TestCase(12, "Parts of Mount Everest")]
        [TestCase(13, "Prime: canonical hour of prayer; prime meridian; prime numbers")]
        [TestCase(14, "Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead")]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenAnswerTextIsCorrect(int questionIndex, string expectedAnswerText)
        {
            Assert.That(_questions[questionIndex].AnswerText, Is.EqualTo(expectedAnswerText));
        }

        [TestCase(0, "Albert Speer")]
        [TestCase(1, "Footballers (academies of <i>Barcelona</i> and <i>Real Madrid</i>)")]
        [TestCase(2, "International Standard Book Number")]
        [TestCase(3, "Danish flag")]
        [TestCase(4, "Der Blaue Reiter (Blue Rider)")]
        [TestCase(5, "East India Company")]
        [TestCase(6, "Suriname")]
        [TestCase(7, "Lady Chatterley’s Lover")]
        [TestCase(8, "Bifrost (bridge in Norse myth, linking gods’ realm and Earth)")]
        [TestCase(9, "Young nicknames: Boy George; Billy the Kid; Babe Ruth; Baby Face Nelson")]
        [TestCase(10, "Attitudes of animals in heraldry: standing; sitting; rearing; walking; lying down")]
        [TestCase(11, "Statues of Boudicca")]
        [TestCase(12, "Parts of Mount Everest")]
        [TestCase(13, "Prime: canonical hour of prayer; prime meridian; prime numbers")]
        [TestCase(14, "Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead")]
        public void GivenQuestionAndAnswerSections_WhenAssembled_ThenAnswerHtmlIsCorrect(int questionIndex, string expectedAnswerHtml)
        {
            Assert.That(_questions[questionIndex].AnswerHtml, Is.EqualTo(expectedAnswerHtml));
        }

        [Test]
        public void GivenQuestionInWrongFormat_WhenAssembled_ThenExceptionIsThrown()
        {
            // Given
            var questionsSection = new List<string>
            {
                "1 Which Nazi leader died in Paddington in 1981?",
                "This shouldn't be here",
                "2 What are produced at <i>La Masia</i> &amp; <i>La Fábrica</i>?"
            };

            var answersSection = new List<string>();
            
            // When
            var exception = Assert.Throws<ParsingException>(() =>
                _questionAssembler.AssembleQuestions(questionsSection, answersSection));
            
            // Then
            Assert.That(exception.Message, Is.EqualTo("Question text in unexpected format: This shouldn't be here"));
        }

        [Test]
        public void GivenQuestionAndAnswerCountsAreNotEqual_WhenAssembled_ThenExceptionIsThrown()
        {
            // Given
            var questionsSection = new List<string>
            {
                "1 Which Nazi leader died in Paddington in 1981?",
                "2 What are produced at <i>La Masia</i> &amp; <i>La Fábrica</i>?",
                "3 In publishing, what does ISBN stand for?",
                "4 Adopted in 1625, what symbol is the Dannebrog?"
            };

            var answersSection = new List<string>
            {
                "1 Albert Speer.",
                "2 Footballers (academies of <i>Barcelona</i> and <i>Real Madrid</i>).",
                "3 International Standard Book Number.",
                "4 Danish flag.",
                "5 Der Blaue Reiter (Blue Rider)."
            };
            
            // When
            var exception = Assert.Throws<ParsingException>(() =>
                _questionAssembler.AssembleQuestions(questionsSection, answersSection));
            
            // Then
            Assert.That(exception.Message, Is.EqualTo("Found 4 questions but 5 answers"));
        }
    }
}