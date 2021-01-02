using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RegexToolbox;
using RegexToolbox.Extensions;
using SaturdayQuizWeb.Model.Parsing;
using SaturdayQuizWeb.Utils;
using static RegexToolbox.RegexOptions;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface ISectionExtractor
    {
        Sections ExtractSections(string wholePageHtml);
    }

    public class SectionExtractor : ISectionExtractor
    {
        private static readonly Regex OpenPTagRegex = new RegexBuilder()
            .HtmlOpenTag("p")
            .BuildRegex(IgnoreCase);

        private static readonly Regex ClosePTagRegex = new RegexBuilder()
            .HtmlCloseTag("p")
            .BuildRegex(IgnoreCase);

        private static readonly Regex QuestionNumberRegex = new RegexBuilder()
            .HtmlOpenTag("strong")
            .AnyCharacter(ZeroOrMore.ButAsFewAsPossible)
            .WordBoundary()
            .Group(r => r
                .Digit(OneOrMore)
            )
            .WordBoundary()
            .AnyCharacter(ZeroOrMore.ButAsFewAsPossible)
            .HtmlCloseTag("strong")
            .BuildRegex(IgnoreCase);

        public Sections ExtractSections(string wholePageHtml)
        {
            var allHtmlLines = wholePageHtml
                .Replace("\n", string.Empty)
                .Replace(OpenPTagRegex, "\n<p>")
                .Replace(ClosePTagRegex, "</p>\n")
                .Split("\n")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(line => line.Trim());

            var paragraphLines = FindParagraphLines(allHtmlLines);

            var sectionLines = paragraphLines
                .Where(IsSectionLine)
                .ToList();

            if (sectionLines.Count > 2)
            {
                sectionLines = StitchSectionLines(sectionLines);
            }

            if (sectionLines.Count != 2)
            {
                throw new ParsingException($"Found {sectionLines.Count} matching line(s) in source HTML (expected 2)");
            }

            return new Sections
            {
                QuestionsSectionHtml = sectionLines[0],
                AnswersSectionHtml = sectionLines[1]
            };
        }

        private static List<string> StitchSectionLines(IEnumerable<string> possiblyBrokenSectionLines)
        {
            var sectionLines = new List<string>();
            foreach (var line in possiblyBrokenSectionLines)
            {
                var firstNumberInLine = QuestionNumberRegex.Match(line).Groups[1].Value;
                if (firstNumberInLine == "1")
                {
                    sectionLines.Add(line);
                }
                else if (sectionLines.Count > 0)
                {
                    sectionLines[sectionLines.Count - 1] += "<br>" + line;
                }
                else
                {
                    throw new ParsingException($"First question found was number {firstNumberInLine}");
                }
            }

            return sectionLines;
        }

        private static IEnumerable<string> FindParagraphLines(IEnumerable<string> wholePageHtmlLines)
        {
            var paragraphLines = wholePageHtmlLines
                .Where(line => line.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (paragraphLines.Count < 2)
            {
                throw new ParsingException($"Found {paragraphLines.Count} lines in <p> tags in source HTML (expected at least 2)");
            }

            return paragraphLines;
        }

        private static bool IsSectionLine(string line) => QuestionNumberRegex.Matches(line).Count >= 3;
    }
}
