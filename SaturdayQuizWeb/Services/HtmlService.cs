using System;
using System.Collections.Generic;
using System.Linq;
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
    public class HtmlService : IHtmlService
    {
        private const int MinQuestionCount = 15;
        private static readonly IEnumerable<int> MinQuestionNumbers = Enumerable.Range(1, MinQuestionCount);

        private static readonly Regex WhatLinksRegex = new RegexBuilder()
            .Text("what")
            .PossibleHtmlWhitespace()
            .Text("links")
            .Text(":", ZeroOrOne)
            .BuildRegex(RegexOptions.IgnoreCase);

        private static readonly Regex ATagRegex = BuildTagRegex("a");
        private static readonly Regex BrTagRegex = BuildTagRegex("br");
        private static readonly Regex PTagRegex = BuildTagRegex("p");
        private static readonly Regex StrongTagRegex = BuildTagRegex("strong");
        private static readonly Regex MultipleSpaceRegex = new RegexBuilder()
            .Whitespace(OneOrMore)
            .BuildRegex();

        private static readonly Regex HtmlTagRegex = new RegexBuilder()
            .Text("<")
            .AnyCharacter(OneOrMore.ButAsFewAsPossible)
            .Text(">")
            .BuildRegex();

        private static readonly Regex RawQuestionAnswerRegex = new RegexBuilder()
            .StartOfString()
            // Question/answer number
            .StartGroup()
            .Digit(OneOrMore)
            .EndGroup()
            .Whitespace(OneOrMore)
            // Question/answer text
            .StartGroup()
            .AnyCharacter(OneOrMore.ButAsFewAsPossible)
            .EndGroup()
            .Text(".", ZeroOrOne)
            .PossibleWhitespace()
            .EndOfString()
            .BuildRegex();
            
        public IEnumerable<Question> FindQuestions(string html)
        {
            var htmlLines = html
                .Split("\n")
                .Select(line => line.Trim())
                .Where(line => line.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
                .Where(line => MinQuestionNumbers.All(number => line.Contains(number.ToString())))
                .Select(SanitiseHtml)
                .ToList();

            if (htmlLines.Count != 2)
            {
                throw new ParsingException($"Found {htmlLines.Count} matching lines in source HTML (expected 2)");
            }

            // Parse questions
            var questions = ParseRawQuestions(htmlLines[0]).ToList();
            
            // Parse answers
            ParseRawAnswers(htmlLines[1], questions);
            
            return questions;
        }

        private static IEnumerable<Question> ParseRawQuestions(string rawQuestionsHtml)
        {
            var questionSections = WhatLinksRegex
                .Split(rawQuestionsHtml)
                .ToList();

            if (questionSections.Count != 2)
            {
                throw new ParsingException($"Found {questionSections.Count} question section(s) (expected 2)");
            }
            
            var normalQuestions = ParseRawQuestionSection(questionSections.First(), QuestionType.Normal);
            var whatLinksQuestions = ParseRawQuestionSection(questionSections.Last(), QuestionType.WhatLinks);

            var questions = new List<Question>();
            questions.AddRange(normalQuestions);
            questions.AddRange(whatLinksQuestions);

            return questions;
        }

        private static IEnumerable<Question> ParseRawQuestionSection(string rawQuestionSectionHtml, QuestionType type)
        {
            var questions = new List<Question>();
            
            var rawQuestions = BrTagRegex
                .Split(rawQuestionSectionHtml)
                .Where(q => !string.IsNullOrWhiteSpace(q));

            foreach (var rawQuestion in rawQuestions)
            {
                var regexMatch = RawQuestionAnswerRegex.Match(rawQuestion);
                if (!regexMatch.Success)
                {
                    throw new ParsingException($"Could not parse {rawQuestion}");
                }
                
                questions.Add(new Question
                {
                    Number = int.Parse(regexMatch.Groups[1].Value),
                    Type = type,
                    QuestionHtml = regexMatch.Groups[2].Value,
                    QuestionText = MakeTextSafe(regexMatch.Groups[2].Value)
                });
            }

            return questions;
        }

        private static void ParseRawAnswers(string rawAnswersHtml, IEnumerable<Question> questions)
        {
            var rawAnswers = BrTagRegex
                .Split(rawAnswersHtml)
                .Where(q => !string.IsNullOrWhiteSpace(q));

            foreach (var rawAnswer in rawAnswers)
            {
                var regexMatch = RawQuestionAnswerRegex.Match(rawAnswer);
                if (!regexMatch.Success)
                {
                    throw new ParsingException($"Could not parse {rawAnswer}");
                }

                var questionNumber = int.Parse(regexMatch.Groups[1].Value);
                var answerHtml = regexMatch.Groups[2].Value;
                var question = questions.Single(q => q.Number == questionNumber);
                question.AnswerHtml = answerHtml;
                question.AnswerText = MakeTextSafe(answerHtml);
            }
        }
        
        private static string SanitiseHtml(string html)
        {
            var sanitisedHtml = html
                .Remove(PTagRegex)
                .Remove(StrongTagRegex)
                .Remove(ATagRegex)
                .Replace("&nbsp;", " ");

            sanitisedHtml = MultipleSpaceRegex.Replace(sanitisedHtml, " ");

            return sanitisedHtml;
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

        private static Regex BuildTagRegex(string tagName) => new RegexBuilder()
            .Text("<")
            .Text("/", ZeroOrOne)
            .Text(tagName)
            .AnyCharacterExcept(">", ZeroOrMore)
            .Text(">")
            .BuildRegex(RegexOptions.IgnoreCase);
    }
}