using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using RegexToolbox;
using RegexToolbox.Extensions;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Utils;
using static RegexToolbox.RegexQuantifier;
using RegexOptions = RegexToolbox.RegexOptions;

namespace SaturdayQuizWeb.Services
{
    public class OldHtmlService : IHtmlService
    {
        private static readonly Regex AnchorTagRegex = new RegexBuilder()
            .Text("<")
            .Text("/", ZeroOrOne)
            .Text("a")
            .AnyCharacterExcept(">", ZeroOrMore)
            .Text(">")
            .BuildRegex();

        private static readonly Regex HtmlTagRegex = new RegexBuilder()
            .Text("<")
            .AnyCharacter(OneOrMore.ButAsFewAsPossible)
            .Text(">")
            .BuildRegex();

        private static readonly Regex WhatLinksRegex = new RegexBuilder()
            .Text("what")
            .HtmlWhitespace()
            .Text("links")
            .BuildRegex(RegexOptions.IgnoreCase);

        private const int MinNumberOfQuestions = 15;

        public IEnumerable<Question> FindQuestions(string html)
        {
            var questionStartIndex = 0;
            var answerStartIndex = 0;
            var questions = new List<Question>();
            var type = QuestionType.Normal;

            for (var number = 1;; number++)
            {
                var regex = BuildRegex(number);

                // Find the question
                var match = regex.Match(html, questionStartIndex);
                if (!match.Success)
                {
                    if (number > MinNumberOfQuestions)
                    {
                        break;
                    }
                    throw new ParsingException($"Failed to find question {number}");
                }

                var prevQuestionStartIndex = questionStartIndex;
                questionStartIndex = match.Index + match.Length;

                if (type == QuestionType.Normal && prevQuestionStartIndex > 0)
                {
                    // Check if we've passed "what links"
                    var previousChunk = html.Substring(
                        prevQuestionStartIndex,
                        questionStartIndex - prevQuestionStartIndex);
                    if (WhatLinksRegex.IsMatch(previousChunk))
                    {
                        type = QuestionType.WhatLinks;
                    }
                }
                
                if (answerStartIndex == 0)
                {
                    answerStartIndex = questionStartIndex;
                }

                var question = match.Groups[1].Value;

                // Find the answer
                match = regex.Match(html, answerStartIndex);
                if (!match.Success)
                {
                    throw new ParsingException($"Failed to find answer {number}");
                }

                answerStartIndex = match.Index + match.Length;
                var answer = match.Groups[1].Value;

                // Check questions and answers are different
                if (question.Equals(answer))
                {
                    throw new ParsingException($"Parsing error: question and answer {number} are the same");
                }
                
                questions.Add(new Question
                {
                    Number = number,
                    Type = type,
                    QuestionText = MakeTextSafe(question),
                    QuestionHtml = MakeHtmlSafe(question),
                    AnswerText = MakeTextSafe(answer),
                    AnswerHtml = MakeHtmlSafe(answer)
                });
            }

            return questions;
        }

        private static Regex BuildRegex(int questionNumber)
        {
            return new RegexBuilder()
                .WordBoundary()
                .Text(questionNumber.ToString())
                .PossibleHtmlWhitespace()
                .Text("</strong").PossibleWhitespace().Text(">")
                .PossibleHtmlWhitespace()
                // Capture group: the question/answer text we want to extract
                .StartGroup()
                .AnyCharacter(OneOrMore.ButAsFewAsPossible)
                .EndGroup()
                // Optional full stop (to remove it from the end of the answer)
                .Text(".", ZeroOrOne)
                .PossibleHtmlWhitespace()
                .Text("<")
                .AnyOf("br", "/p", "p", "/strong")
                .PossibleWhitespace()
                .Text("/", ZeroOrOne)
                .Text(">")
                .BuildRegex();
        }

        private static string MakeHtmlSafe(string source)
        {
            return source.Remove(AnchorTagRegex);
        }

        private static string MakeTextSafe(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            
            var safeSource = HttpUtility.HtmlDecode(source);
            return safeSource.Remove(HtmlTagRegex);
        }
    }
}