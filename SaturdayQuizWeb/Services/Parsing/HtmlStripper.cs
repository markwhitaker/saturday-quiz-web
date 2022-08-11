using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RegexToolbox;
using RegexToolbox.Extensions;
using SaturdayQuizWeb.Extensions;
using static RegexToolbox.RegexOptions;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services.Parsing;

public interface IHtmlStripper
{
    string StripHtml(string htmlString);
}

public class HtmlStripper : IHtmlStripper
{
    private static readonly IEnumerable<string> TagsToStrip = new[]
    {
        "a", "b", "cite", "code", "em", "p", "s", "small", "span", "strong", "sub", "sup", "u"
    };

    private static readonly IEnumerable<Regex> TagRegexes = TagsToStrip.Select(BuildTagRegex);
    private static readonly Regex BrTagRegex = BuildBrTagRegex();

    public string StripHtml(string htmlString)
    {
        var strippedText = TagRegexes.Aggregate(
            htmlString,
            (current, tagRegex) => current.Remove(tagRegex));

        strippedText = BrTagRegex.Replace(strippedText, "\n");
        strippedText = strippedText
            .Replace("&nbsp;", " ")
            .Replace("\u00A0", " "); // Unicode non-breaking space

        return strippedText;
    }

    private static Regex BuildTagRegex(string tagName) => new RegexBuilder()
        .Text("<")
        .Text("/", ZeroOrOne)
        .Text(tagName)
        .WordBoundary()
        .AnyCharacterExcept(">", ZeroOrMore)
        .Text(">")
        .BuildRegex(IgnoreCase);

    private static Regex BuildBrTagRegex() => new RegexBuilder()
        .PossibleHtmlWhitespace()
        .Text("<")
        .Text("/", ZeroOrOne)
        .Text("br")
        .WordBoundary()
        .AnyCharacterExcept(">", ZeroOrMore)
        .Text(">")
        .PossibleHtmlWhitespace()
        .BuildRegex(IgnoreCase);
}
