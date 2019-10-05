using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegexToolbox;
using SaturdayQuizWeb.Model;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services
{
    public class HtmlException : Exception
    {
        public HtmlException(string message) : base(message)
        {
        }
    }

    public interface IHtmlService
    {
        List<Question> FindQuestions(string html);
    }

    public class HtmlService : IHtmlService
    {
        private const int NumberOfQuestions = 15;
        private const int NumberOfNormalQuestions = 8;

        public List<Question> FindQuestions(string html)
        {
            var questionStartIndex = 0;
            var answerStartIndex = 0;
            var questions = new List<Question>();

            for (var number = 1; number <= NumberOfQuestions; number++)
            {
                var regex = BuildRegex(number);

                // Find the question
                var match = regex.Match(html, questionStartIndex);
                if (!match.Success)
                {
                    throw new HtmlException($"Failed to find question {number}");
                }

                questionStartIndex = match.Index + match.Length;
                if (answerStartIndex == 0)
                {
                    answerStartIndex = questionStartIndex;
                }

                var question = match.Groups[1].Value;

                // Find the answer
                match = regex.Match(html, answerStartIndex);
                if (!match.Success)
                {
                    throw new HtmlException($"Failed to find answer {number}");
                }

                answerStartIndex = match.Index + match.Length;
                var answer = match.Groups[1].Value;
                
                questions.Add(new Question
                {
                    Number = number,
                    Type = GetQuestionType(number),
                    QuestionText = question,
                    Answer = answer
                });
            }

            return questions;
        }

        private static Regex BuildRegex(int questionNumber)
        {
            return new RegexBuilder()
                .WordBoundary()
                .Text(questionNumber.ToString())
                .PossibleWhitespace()
                .Text("</strong").PossibleWhitespace().Text(">")
                .PossibleWhitespace()
                // Capture group: the question/answer text we want to extract
                .StartGroup()
                .AnyCharacter(OneOrMore.ButAsFewAsPossible)
                .EndGroup()
                // Optional full stop (to remove it from the end of the answer)
                .Text(".", ZeroOrOne)
                .PossibleWhitespace()
                .Text("<")
                .AnyOf("br", "/p", "p", "/strong")
                .PossibleWhitespace()
                .Text("/", ZeroOrOne)
                .Text(">")
                .BuildRegex();
        }

        private static QuestionType GetQuestionType(int questionNumber)
        {
            return questionNumber <= NumberOfNormalQuestions ? QuestionType.Normal : QuestionType.WhatLinks;
        }
    }
}