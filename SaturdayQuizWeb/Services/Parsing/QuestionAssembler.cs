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
        IEnumerable<Question> AssembleQuestions(IEnumerable<string> questionsSection, IEnumerable<string> answersSection);
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

        public IEnumerable<Question> AssembleQuestions(IEnumerable<string> questionsSection, IEnumerable<string> answersSection)
        {
            var questions = ProcessQuestionsSection(questionsSection);
            ProcessAnswersSection(answersSection, questions);
            AddPlainTextStrings(questions);
            
            return questions;
        }

        private static IList<Question> ProcessQuestionsSection(IEnumerable<string> questionsSection)
        {
            var questions = new List<Question>();

            var questionType = QuestionType.Normal;
            foreach (var question in questionsSection)
            {
                if (question.StartsWith("what links", StringComparison.OrdinalIgnoreCase))
                {
                    questionType = QuestionType.WhatLinks;
                    continue;
                }

                var match = NumberTextRegex.Match(question);
                if (!match.Success)
                {
                    throw new ParsingException($"Question text in unexpected format: {question}");
                }

                questions.Add(new Question
                {
                    Number = int.Parse(match.Groups[GroupNameNumber].Value),
                    QuestionHtml = match.Groups[GroupNameText].Value,
                    Type = questionType
                });
            }

            return questions;
        }

        private static void ProcessAnswersSection(IEnumerable<string> answersSection, IList<Question> questions)
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

                questions[i].AnswerHtml = match.Groups[GroupNameText].Value;
            }
        }
        
        private void AddPlainTextStrings(IList<Question> questions)
        {
            foreach (var question in questions)
            {
                question.QuestionText = MakeTextSafe(question.QuestionHtml);
                question.AnswerText = MakeTextSafe(question.AnswerHtml);
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