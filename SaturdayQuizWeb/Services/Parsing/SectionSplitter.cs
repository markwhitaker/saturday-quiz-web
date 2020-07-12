using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RegexToolbox;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface ISectionSplitter
    {
        IEnumerable<string> SplitSection(string section);
    }

    public class SectionSplitter : ISectionSplitter
    {
        private const string WhatLinks = "what links";

        private static readonly Regex MultipleWhitespaceRegex = new RegexBuilder()
            .Whitespace(AtLeast(2))
            .BuildRegex();
        
        public IEnumerable<string> SplitSection(string section)
        {
            var splitSection = section.Split("\n").ToList();

            // Look for mid-question/answer splits
            var expectedNextQuestionNumber = 1;
            for (var index = 0; index < splitSection.Count; index++)
            {
                if (splitSection[index].StartsWith(WhatLinks, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (splitSection[index].StartsWith(expectedNextQuestionNumber + " "))
                {
                    expectedNextQuestionNumber++;
                    continue;
                }

                if (index == 0)
                {
                    splitSection.RemoveAt(0);
                }
                else
                {
                    splitSection[index - 1] = string.Join(
                        " ",
                        splitSection[index - 1],
                        splitSection[index]);
                    splitSection.RemoveAt(index);
                    index--;
                }
            }

            // Trim any multiple spaces
            splitSection = splitSection
                .Select(text => MultipleWhitespaceRegex.Replace(text, " "))
                .ToList();
            
            return splitSection;
        }
    }
}