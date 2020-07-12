using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RegexToolbox;
using RegexToolbox.Extensions;
using SaturdayQuizWeb.Utils;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services.Parsing
{
    public interface IHtmlStripper
    {
        string StripHtml(string htmlString);
    }

    public class HtmlStripper : IHtmlStripper
    {
        private static readonly IEnumerable<string> TagsToStrip = new[]
        {
            "a", "b", "cite", "code", "p", "s", "small", "span", "strong", "sub", "sup", "u"
        };
        private static readonly IEnumerable<Regex> TagRegexes = TagsToStrip.Select(BuildTagRegex);
        private static readonly Regex BrTagRegex = BuildBrTagRegex();

        public string StripHtml(string htmlString)
        {
            var strippedText = TagRegexes.Aggregate(
                htmlString,
                (current, tagRegex) => current.Remove(tagRegex));

            strippedText = BrTagRegex.Replace(strippedText, "\n");
            strippedText = strippedText.Replace("&nbsp;", " ");
            
            return strippedText;
        }
        
        private static Regex BuildTagRegex(string tagName) => new RegexBuilder()
            .Text("<")
            .Text("/", ZeroOrOne)
            .Text(tagName)
            .WordBoundary()
            .AnyCharacterExcept(">", ZeroOrMore)
            .Text(">")
            .BuildRegex(RegexToolbox.RegexOptions.IgnoreCase);
        
        private static Regex BuildBrTagRegex() => new RegexBuilder()
            .PossibleHtmlWhitespace()
            .Text("<")
            .Text("/", ZeroOrOne)
            .Text("br")
            .WordBoundary()
            .AnyCharacterExcept(">", ZeroOrMore)
            .Text(">")
            .PossibleHtmlWhitespace()
            .BuildRegex(RegexToolbox.RegexOptions.IgnoreCase);
    }
}