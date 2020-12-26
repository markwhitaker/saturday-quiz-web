using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RegexToolbox;
using SaturdayQuizWeb.Utils;
using RegexOptions = RegexToolbox.RegexOptions;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface ISectionExtractor
    {
        IEnumerable<string> ExtractSectionParagraphs(string wholePageHtml);
    }

    public class SectionExtractor : ISectionExtractor
    {
        private static readonly Regex PTagRegex = new RegexBuilder()
            .Text("<p")
            .WordBoundary()
            .AnyCharacterExcept(">", RegexQuantifier.ZeroOrMore)
            .Text(">")
            .BuildRegex(RegexOptions.IgnoreCase);

        public IEnumerable<string> ExtractSectionParagraphs(string wholePageHtml)
        {
            var allHtmlLines = wholePageHtml
                .Replace("\n", string.Empty)
                .Replace(PTagRegex, "\n<p>")
                .Replace("</p>", "</p>\n")
                .Split("\n")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(line => line.Trim());

            var allParagraphs = allHtmlLines
                .Where(line => line.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
                .ToList();

            return allParagraphs;

            // var sectionParagraphs = allParagraphs
            //     .Where(IsSectionLine)
            //     .ToList();
            //
            // return sectionParagraphs;
        }

        private static bool IsSectionLine(string line)
        {
            var index = 0;
            for (var number = 1; number <= ParsingConstants.MinimumQuestionCount; number++)
            {
                index = line.IndexOf(number.ToString(), index, StringComparison.Ordinal);
                if (index == -1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
