using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using RegexToolbox;
using SaturdayQuizWeb.Model.Parsing;
using static RegexToolbox.RegexQuantifier;
using RegexOptions = System.Text.RegularExpressions.RegexOptions;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface ISectionSplitter
    {
        Sections SplitSections(IEnumerable<string> paragraphs);
    }

    public class SectionSplitter : ISectionSplitter
    {
        private static readonly Regex SectionParagraphRegex = new Regex(@"^(what links|(\d+)\s)", RegexOptions.IgnoreCase);

        private static readonly Regex MultipleWhitespaceRegex = new RegexBuilder()
            .Whitespace(OneOrMore)
            .BuildRegex();

        private static readonly Regex QuestionRegex = new Regex(@"(what links|(\d+)\s+.+?\?)", RegexOptions.IgnoreCase);

        private static readonly Regex AnswerRegex = new RegexBuilder()
            .StartGroup()
            .Digit(OneOrMore)
            .EndGroup()
            .Whitespace(OneOrMore)
            .AnyCharacter(OneOrMore.ButAsFewAsPossible)
            .Text(".")
            .BuildRegex();

        private static readonly Regex StartsWithNumberRegex = new RegexBuilder()
            .StartOfString()
            .StartGroup()
            .Digit(OneOrMore)
            .EndGroup()
            .Whitespace()
            .BuildRegex();

        public Sections SplitSections(IEnumerable<string> paragraphs)
        {
            var unsplitSections = paragraphs
                .Where(p => SectionParagraphRegex.IsMatch(p))
                .ToList();

            var sections = new Sections();

            var addingToSection = sections.QuestionsSection;
            var regex = QuestionRegex;

            // Find questions
            foreach (var unsplitSection in unsplitSections)
            {
                // Switch to answers section when we get to it
                if (addingToSection.Count > 0 && unsplitSection.StartsWith("1 "))
                {
                    addingToSection = sections.AnswersSection;
                    regex = AnswerRegex;
                }

                var splitSection = SplitSection(unsplitSection, regex);
                addingToSection.AddRange(splitSection);
            }

            return sections;
        }

        private static IEnumerable<string> SplitSection(string unsplitSection, Regex regex)
        {
            // // Find the index of "what links" and remove it from the string
            // var whatLinksIndex = unsplitSection.IndexOf(ParsingConstants.WhatLinks, StringComparison.OrdinalIgnoreCase);
            // if (whatLinksIndex > -1)
            // {
            //     unsplitSection = unsplitSection.Remove(whatLinksIndex, ParsingConstants.WhatLinks.Length);
            // }
            // unsplitSection = unsplitSection.Trim();
            //
            // var numberMatch = StartsWithNumberRegex.Match(unsplitSection);
            // if (!numberMatch.Success)
            // {
            //     throw new ParsingException($"Not a valid question, answer or What Links separator: {unsplitSection}");
            // }
            //
            // var splitSection = new List<string>();
            // var needToInsertWhatLinks = whatLinksIndex > -1;
            // var numberIndex = 0;
            // for (var number = int.Parse(numberMatch.Groups[1].Value); true; number++)
            // {
            //     var numberIndex =
            //     var nextNumberAsString = (number + 1).ToString();
            //     var nextNumberIndex = unsplitSection.IndexOf(nextNumberAsString, StringComparison.Ordinal);
            //
            //     if (needToInsertWhatLinks && nextNumberIndex > whatLinksIndex)
            //     {
            //         needToInsertWhatLinks = false;
            //     }
            //     break;
            // }

            var allMatches = regex.Matches(unsplitSection);
            if (allMatches.Count == 0)
            {
                throw new ParsingException($"Not a valid question, answer or What Links separator: {unsplitSection}");
            }

            return allMatches
                .Select(match => MultipleWhitespaceRegex.Replace(match.Value, " "))
                .ToList();
        }
    }
}
