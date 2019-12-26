using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.UnitTests.Services
{
    [TestFixture]
    public class HtmlServiceTest
    {
        // Object under test
        private readonly HtmlService _htmlService = new HtmlService();
        
        private List<Question> _questions;

        [SetUp]
        public void SetUp()
        {
            var html = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "/TestData/2019_07_20_quiz.html");
            _questions = _htmlService.FindQuestions(html);
        }
        
        [Test]
        public void TestQuestionCount()
        {
            Assert.AreEqual(15, _questions.Count);
        }

        [Test]
        public void TestQuestion1()
        {
            Assert.AreEqual(1, _questions[0].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[0].Type);
            Assert.AreEqual("Which Nazi leader died in Paddington in 1981?", _questions[0].QuestionText);
            Assert.AreEqual("Albert Speer", _questions[0].Answer);
        }

        [Test]
        public void TestQuestion2()
        {
            Assert.AreEqual(2, _questions[1].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[1].Type);
            // Check that normal HTML tags are allowed
            Assert.AreEqual("What are produced at <i>La Masia</i> and <i>La Fábrica</i>?", _questions[1].QuestionText);
            // Check that <a> tags are removed
            Assert.AreEqual("Footballers (academies of Barcelona and Real Madrid)", _questions[1].Answer);
        }

        [Test]
        public void TestQuestion3()
        {
            Assert.AreEqual(3, _questions[2].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[2].Type);
            Assert.AreEqual("In publishing, what does ISBN stand for?", _questions[2].QuestionText);
            Assert.AreEqual("International Standard Book Number", _questions[2].Answer);
        }

        [Test]
        public void TestQuestion4()
        {
            Assert.AreEqual(4, _questions[3].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[3].Type);
            Assert.AreEqual("Adopted in 1625, what symbol is the Dannebrog?", _questions[3].QuestionText);
            Assert.AreEqual("Danish flag", _questions[3].Answer);
        }

        [Test]
        public void TestQuestion5()
        {
            Assert.AreEqual(5, _questions[4].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[4].Type);
            Assert.AreEqual("Gabriele Münter was a founder member of what expressionist group?", _questions[4].QuestionText);
            Assert.AreEqual("Der Blaue Reiter (Blue Rider)", _questions[4].Answer);
        }

        [Test]
        public void TestQuestion6()
        {
            Assert.AreEqual(6, _questions[5].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[5].Type);
            Assert.AreEqual("What was nicknamed the Honourable John Company?", _questions[5].QuestionText);
            Assert.AreEqual("East India Company", _questions[5].Answer);
        }

        [Test]
        public void TestQuestion7()
        {
            Assert.AreEqual(7, _questions[6].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[6].Type);
            Assert.AreEqual("Which country separates Guyana and French Guiana?", _questions[6].QuestionText);
            Assert.AreEqual("Suriname", _questions[6].Answer);
        }

        [Test]
        public void TestQuestion8()
        {
            Assert.AreEqual(8, _questions[7].Number);
            Assert.AreEqual(QuestionType.Normal, _questions[7].Type);
            Assert.AreEqual("In what novel is Constance unhappily married to Sir Clifford?", _questions[7].QuestionText);
            Assert.AreEqual("Lady Chatterley’s Lover", _questions[7].Answer);
        }

        [Test]
        public void TestQuestion9()
        {
            Assert.AreEqual(9, _questions[8].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[8].Type);
            Assert.AreEqual("Asgard and Midgard, in the form of a rainbow?", _questions[8].QuestionText);
            Assert.AreEqual("Bifrost (bridge in Norse myth, linking gods’ realm and Earth)", _questions[8].Answer);
        }

        [Test]
        public void TestQuestion10()
        {
            Assert.AreEqual(10, _questions[9].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[9].Type);
            Assert.AreEqual("Singer O’Dowd; outlaw McCarty; slugger Ruth; bank robber Nelson?", _questions[9].QuestionText);
            Assert.AreEqual("Young nicknames: Boy George; Billy the Kid; Babe Ruth; Baby Face Nelson", _questions[9].Answer);
        }

        [Test]
        public void TestQuestion11()
        {
            Assert.AreEqual(11, _questions[10].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[10].Type);
            Assert.AreEqual("Statant; sejant; rampant; passant; dormant?", _questions[10].QuestionText);
            Assert.AreEqual("Attitudes of animals in heraldry: standing; sitting; rearing; walking; lying down", _questions[10].Answer);
        }

        [Test]
        public void TestQuestion12()
        {
            Assert.AreEqual(12, _questions[11].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[11].Type);
            Assert.AreEqual("Victoria Embankment; Cardiff City Hall; Colchester station?", _questions[11].QuestionText);
            Assert.AreEqual("Statues of Boudicca", _questions[11].Answer);
        }

        [Test]
        public void TestQuestion13()
        {
            Assert.AreEqual(13, _questions[12].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[12].Type);
            Assert.AreEqual("Khumbu icefall; Kangshung face; Hornbein couloir; Hillary step?", _questions[12].QuestionText);
            Assert.AreEqual("Parts of Mount Everest", _questions[12].Answer);
        }

        [Test]
        public void TestQuestion14()
        {
            Assert.AreEqual(14, _questions[13].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[13].Type);
            Assert.AreEqual("Prayers at 6am; 0 degrees longitude; 2, 3, 5, 7, etc?", _questions[13].QuestionText);
            Assert.AreEqual("Prime: canonical hour of prayer; prime meridian; prime numbers", _questions[13].Answer);
        }

        [Test]
        public void TestQuestion15()
        {
            Assert.AreEqual(15, _questions[14].Number);
            Assert.AreEqual(QuestionType.WhatLinks, _questions[14].Type);
            Assert.AreEqual("Prince of Morocco (Au); Prince of Arragon (Ag); Bassanio (Pb)?", _questions[14].QuestionText);
            Assert.AreEqual("Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead", _questions[14].Answer);
        }
    }
}