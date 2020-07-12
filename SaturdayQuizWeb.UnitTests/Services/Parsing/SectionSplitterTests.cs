using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.UnitTests.Services.Parsing
{
    [TestFixture]
    public class SectionSplitterTests
    {
        private readonly ISectionSplitter _sectionSplitter = new SectionSplitter();

        [Test]
        public void GivenQuestionsSectionText_WhenSplit_ThenExpectedQuestionsAreReturned()
        {
            // Given
            const string questionsSection =
                "1 Which Nazi leader died in Paddington in 1981?\n" +
                "2 What are produced at <i>La Masia</i> &amp; <i>La Fábrica</i>?\n" +
                "3 In publishing, what does ISBN stand for?\n" +
                "4 Adopted in 1625, what symbol is the Dannebrog?\n" +
                "5 Gabriele Münter was a founder member of what expressionist group?\n" +
                "6 What was nicknamed the Honourable John Company?\n" +
                "7 Which country separates Guyana and French Guiana?\n" +
                "8 In what novel is Constance unhappily married to Sir Clifford?\n" +
                "What links:\n" +
                "9 Asgard and Midgard, in the form of a rainbow?\n" +
                "10 Singer O’Dowd; outlaw McCarty; slugger Ruth; bank robber Nelson?\n" +
                "11 Statant; sejant; rampant; passant; dormant?\n" +
                "12 Victoria Embankment; Cardiff City Hall; Colchester station?\n" +
                "13 Khumbu icefall; Kangshung face; Hornbein couloir; Hillary step?\n" +
                "14 Prayers at 6am; 0 degrees longitude; 2, 3, 5, 7, etc?\n" +
                "15 Prince of Morocco (Au); Prince of Arragon (Ag); Bassanio (Pb)?";

            var expectedSplitSection = new List<string>
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
            
            // When
            var actualSplitSection = _sectionSplitter.SplitSection(questionsSection).ToList();
            
            // Then
            Assert.That(actualSplitSection, Is.EqualTo(expectedSplitSection));
        }

        [Test]
        public void GivenAnswersSectionText_WhenSplit_ThenExpectedAnswersAreReturned()
        {
            // Given
            const string answersSection =
                "1 Albert Speer.\n" +
                "2 Footballers (academies of <i>Barcelona</i> and <i>Real Madrid</i>).\n" +
                "3 International Standard Book Number.\n" +
                "4 Danish flag.\n" +
                "5 Der Blaue Reiter (Blue Rider).\n" +
                "6 East India Company.\n" +
                "7 Suriname.\n" +
                "8 Lady Chatterley’s Lover.\n" +
                "9 Bifrost (bridge in Norse myth, linking gods’ realm and Earth).\n" +
                "10 Young nicknames: Boy George; Billy the Kid; Babe Ruth; Baby Face Nelson.\n" +
                "11 Attitudes of animals in heraldry: standing; sitting; rearing;\n" +
                " walking;\n" +
                " lying down.\n" +
                "12 Statues of Boudicca.\n" +
                "13 Parts of Mount Everest.\n" +
                "14 Prime: canonical hour of prayer; prime meridian; prime numbers.\n" +
                "15 Caskets chosen by Portia’s suitors in The Merchant Of Venice:\n" +
                " gold; silver; lead.";
            
            var expectedSplitSection = new List<string>
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
                        
            // When
            var actualSplitSection = _sectionSplitter.SplitSection(answersSection).ToList();
            
            // Then
            Assert.That(actualSplitSection, Is.EqualTo(expectedSplitSection));
        }
    }
}