using System;
using System.Collections.Generic;
using System.Linq;
using SaturdayQuizWeb.Model.Parsing;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface ISectionExtractor
    {
        Sections ExtractSections(string wholePageHtml);
    }

    public class SectionExtractor : ISectionExtractor
    {
        private const int MinQuestionCount = 10;
        private static readonly IEnumerable<int> MinQuestionNumbers = Enumerable.Range(1, MinQuestionCount);

        public Sections ExtractSections(string wholePageHtml)
        {
            var sectionLines = wholePageHtml
                .Split("\n")
                .Select(line => line.Trim())
                .Where(line => line.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
                .Where(line => MinQuestionNumbers.All(number => line.Contains(number.ToString())))
                .ToList();

            if (sectionLines.Count != 2)
            {
                throw new ParsingException($"Found {sectionLines.Count} matching lines in source HTML (expected 2)");
            }

            return new Sections
            {
                QuestionsSectionHtml = sectionLines[0],
                AnswersSectionHtml = sectionLines[1]
            };
        }
    }
}