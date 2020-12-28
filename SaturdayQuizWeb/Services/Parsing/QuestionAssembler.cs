using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using RegexToolbox;
using RegexToolbox.Extensions;
using SaturdayQuizWeb.Model;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface IQuestionAssembler
    {
        IEnumerable<QuestionModel> AssembleQuestions(IEnumerable<string> questionsSection, IEnumerable<string> answersSection);
    }

    public class QuestionAssembler : IQuestionAssembler
    {
        private const string GroupNameNumber = "number";
        private const string GroupNameText = "text";

        private static readonly Regex NumberTextRegex = new RegexBuilder()
            .StartNamedGroup(GroupNameNumber)
            .Digit(OneOrMore)
            .EndGroup()
            .Whitespace(OneOrMore)
            .StartNamedGroup(GroupNameText)
            .AnyCharacter(OneOrMore.ButAsFewAsPossible)
            .EndGroup()
            .Text(".", ZeroOrOne)
            .EndOfString()
            .BuildRegex();

        private static readonly Regex HtmlTagRegex = new RegexBuilder()
            .Text("<")
            .AnyCharacter(OneOrMore.ButAsFewAsPossible)
            .Text(">")
            .BuildRegex();

        public IEnumerable<QuestionModel> AssembleQuestions(IEnumerable<string> questionsSection, IEnumerable<string> answersSection)
        {
            var questions = ProcessQuestionsSection(questionsSection);
            ProcessAnswersSection(answersSection, questions);
            return questions;
        }

        private static IList<QuestionModel> ProcessQuestionsSection(IEnumerable<string> questionsSection)
        {
            var questions = new List<QuestionModel>();

            var questionType = QuestionType.Normal;
            foreach (var question in questionsSection)
            {
                if (question.StartsWith(ParsingConstants.WhatLinks, StringComparison.OrdinalIgnoreCase))
                {
                    questionType = QuestionType.WhatLinks;
                    continue;
                }

                var match = NumberTextRegex.Match(question);
                if (!match.Success)
                {
                    throw new ParsingException($"Question text in unexpected format: {question}");
                }

                questions.Add(new QuestionModel
                {
                    Number = int.Parse(match.Groups[GroupNameNumber].Value),
                    Question = MakeTextSafe(match.Groups[GroupNameText].Value),
                    Type = questionType
                });
            }

            return questions;
        }

        private static void ProcessAnswersSection(IEnumerable<string> answersSection, IList<QuestionModel> questions)
        {
            var answersSectionList = answersSection.ToList();
            if (answersSectionList.Count != questions.Count)
            {
                throw new ParsingException($"Found {questions.Count} questions but {answersSectionList.Count} answers");
            }

            for (var i = 0; i < answersSectionList.Count; i++)
            {
                var answer = answersSectionList[i];
                var match = NumberTextRegex.Match(answer);
                if (!match.Success)
                {
                    throw new ParsingException($"Answer text in unexpected format: {answer}");
                }

                questions[i].Answer = MakeTextSafe(match.Groups[GroupNameText].Value);
            }
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
