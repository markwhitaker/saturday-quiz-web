using System.IO;
using NUnit.Framework;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.UnitTests.Services.Parsing
{
    [TestFixture]
    public class SectionExtractorTests
    {
        private readonly ISectionExtractor _sectionExtractor = new SectionExtractor();
        private string _wholePageHtml;

        public SectionExtractorTests()
        {
            _wholePageHtml = File.ReadAllText(
                TestContext.CurrentContext.TestDirectory + "/TestData/2019_07_20_quiz.html");
        }

        [Test]
        public void GivenWholePageHtml_WhenSectionsAreExtracted_ThenExpectedSectionsAreReturned()
        {
            // Given
            const string expectedQuestionsSectionHtml =
                "<p><strong>1</strong> Which Nazi leader died in Paddington in 1981?<br>" +
                "<strong>2</strong> What are produced at <i>La Masia</i> &amp; <i>La Fábrica</i>?<br>" +
                "<strong>3</strong> In publishing, what does ISBN stand for?<br>" +
                "<strong>4</strong> Adopted in 1625, what symbol is the Dannebrog?<br>" +
                "<strong>5</strong> Gabriele Münter was a founder member of what expressionist group?<br>" +
                "<strong>6</strong> What was nicknamed the Honourable John Company?<br>" +
                "<strong>7</strong> Which country separates Guyana and French Guiana?<br>" +
                "<strong>8</strong> In what novel is Constance unhappily married to Sir Clifford?<br>" +
                "<strong>What links:</strong><br>" +
                "<strong>9</strong> Asgard and Midgard, in the form of a rainbow?<br>" +
                "<strong>10</strong> Singer O’Dowd; outlaw McCarty; slugger Ruth; bank robber Nelson?<br>" +
                "<strong>11</strong> Statant; sejant; rampant; passant; dormant?<br>" +
                "<strong>12</strong> Victoria Embankment; Cardiff City Hall; Colchester station?<br>" +
                "<strong>13</strong> Khumbu icefall; Kangshung face; Hornbein couloir; Hillary step?<br>" +
                "<strong>14</strong> Prayers at 6am; 0 degrees longitude; 2, 3, 5, 7, etc?<br>" +
                "<strong>15</strong> Prince of Morocco (Au); Prince of Arragon (Ag); Bassanio (Pb)?</p>";
            const string expectedAnswersSectionHtml =
                "<p><strong>1</strong> Albert Speer. <br>" +
                "<strong>2</strong> <a href=\"https://www.theguardian.com/\">Footballers (academies of <i>Barcelona</i> and <i>Real Madrid</i>)</a>. <br>" +
                "<strong>3</strong> International Standard Book Number. <br>" +
                "<strong>4</strong> Danish flag. <br>" +
                "<strong>5</strong> Der Blaue Reiter (Blue Rider). <br>" +
                "<strong>6</strong> East India Company. <br>" +
                "<strong>7</strong> Suriname. <br>" +
                "<strong>8</strong> Lady Chatterley’s Lover. <br>" +
                "<strong>9</strong> Bifrost (bridge in Norse myth, linking gods’ realm and Earth). <br>" +
                "<strong>10</strong> Young nicknames: Boy George; Billy the Kid; Babe Ruth; Baby Face Nelson. <br>" +
                "<strong>11</strong> Attitudes of animals in heraldry: standing; sitting; rearing; walking; lying down. <br>" +
                "<strong>12</strong> Statues of Boudicca. <br>" +
                "<strong>13</strong> Parts of Mount Everest. <br>" +
                "<strong>14</strong> Prime: canonical hour of prayer; prime meridian; prime numbers. <br>" +
                "<strong>15</strong> Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead.</p>";
            
            // When
            var sections = _sectionExtractor.ExtractSections(_wholePageHtml);

            // Then
            Assert.That(sections, Is.Not.Null);
            Assert.That(sections.QuestionsSectionHtml, Is.EqualTo(expectedQuestionsSectionHtml));
            Assert.That(sections.AnswersSectionHtml, Is.EqualTo(expectedAnswersSectionHtml));
        }
    }
}