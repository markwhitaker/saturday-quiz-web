using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.UnitTests.Services
{
    [TestFixture]
    public class HtmlServiceTest
    {
        private readonly IHtmlService _htmlService = new HtmlService();
        private readonly string _html;
        private IList<Question> _questions;

        public HtmlServiceTest()
        {
            _html = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "/TestData/2019_07_20_quiz.html");
        }

        [SetUp]
        public void SetUp()
        {
            _questions = _htmlService.FindQuestions(_html).ToList();
        }
        
        [Test]
        public void GivenQuizIsLoaded_WhenQuestionCount_ThenCountIs15()
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
        public void GivenQuizIsLoaded_WhenQuestionNumber_ThenNumberIsCorrect(int questionIndex, int expectedQuestionNumber)
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
        public void GivenQuizIsLoaded_WhenQuestionType_ThenTypeIsCorrect(int questionIndex, QuestionType expectedQuestionType)
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
        public void GivenQuizIsLoaded_WhenQuestionText_ThenTextIsCorrect(int questionIndex, string expectedQuestionText)
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
        public void GivenQuizIsLoaded_WhenQuestionHtml_ThenHtmlIsCorrect(int questionIndex, string expectedQuestionHtml)
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
        public void GivenQuizIsLoaded_WhenAnswerText_ThenTextIsCorrect(int questionIndex, string expectedAnswerText)
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
        public void GivenQuizIsLoaded_WhenAnswerHtml_ThenHtmlIsCorrect(int questionIndex, string expectedAnswerHtml)
        {
            Assert.That(_questions[questionIndex].AnswerHtml, Is.EqualTo(expectedAnswerHtml));
        }
    }
}