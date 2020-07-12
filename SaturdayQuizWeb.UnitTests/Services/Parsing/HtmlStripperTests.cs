using NUnit.Framework;
using SaturdayQuizWeb.Services.Parsing;

namespace SaturdayQuizWeb.UnitTests.Services.Parsing
{
    [TestFixture]
    public class HtmlStripperTests
    {
        private readonly IHtmlStripper _htmlStripper = new HtmlStripper();

        [Test]
        public void GivenHtmlText_WhenHtmlIsStripped_ThenExpectedTagsAreRemoved()
        {
            // Given
            const string questionsSectionHtml =
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
            const string answersSectionHtml =
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
                "<strong>11</strong> Attitudes of animals in heraldry: standing; sitting; rearing; <br>walking; lying down. <br>" +
                "<strong>12</strong> Statues of Boudicca. <br>" +
                "<strong>13</strong> Parts of Mount Everest. <br>" +
                "<strong>14</strong> Prime: canonical hour of prayer; prime meridian; prime numbers. <br>" +
                "<strong>15</strong> Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead.</p>";

            const string expectedQuestionsSection =
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
            const string expectedAnswersSection =
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
                "walking; lying down.\n" +
                "12 Statues of Boudicca.\n" +
                "13 Parts of Mount Everest.\n" +
                "14 Prime: canonical hour of prayer; prime meridian; prime numbers.\n" +
                "15 Caskets chosen by Portia’s suitors in The Merchant Of Venice: gold; silver; lead.";

            // When
            var actualQuestionsSection = _htmlStripper.StripHtml(questionsSectionHtml);
            var actualAnswersSection = _htmlStripper.StripHtml(answersSectionHtml);
            
            // Then
            Assert.That(actualQuestionsSection, Is.EqualTo(expectedQuestionsSection));
            Assert.That(actualAnswersSection, Is.EqualTo(expectedAnswersSection));
        }
    }
}