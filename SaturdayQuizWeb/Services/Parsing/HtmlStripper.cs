using RegexToolbox;
using RegexToolbox.Extensions;
using SaturdayQuizWeb.Extensions;
using static RegexToolbox.RegexOptions;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Services.Parsing;

public class HtmlStripper : IHtmlStripper
{
    private static readonly Regex UnwantedTagsRegex = new RegexBuilder()
        .Text("<")
        .Text("/", ZeroOrOne)
        .AnyOf("a", "b", "cite", "code", "em", "p", "s", "small", "span", "strong", "sub", "sup", "u")
        .WordBoundary()
        .AnyCharacterExcept(">", ZeroOrMore)
        .Text(">")
        .BuildRegex(IgnoreCase);

    private static readonly Regex BrTagRegex = new RegexBuilder()
        .PossibleHtmlWhitespace()
        .Text("<")
        .Text("/", ZeroOrOne)
        .Text("br")
        .WordBoundary()
        .AnyCharacterExcept(">", ZeroOrMore)
        .Text(">")
        .PossibleHtmlWhitespace()
        .BuildRegex(IgnoreCase);

    public string StripHtml(string htmlString) =>
        htmlString.Remove(UnwantedTagsRegex)
            .Replace(BrTagRegex, "\n")
            .Replace("&nbsp;", " ")
            .Replace("\u00A0", " "); // Unicode non-breaking space
}
