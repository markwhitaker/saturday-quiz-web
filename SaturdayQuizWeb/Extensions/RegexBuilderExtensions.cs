using RegexToolbox;

namespace SaturdayQuizWeb.Extensions;

public static class RegexBuilderExtensions
{
    private static readonly IEnumerable<string> HtmlWhitespaceCharacters = [" ", @"\t", @"\n", @"\r", "&nbsp;"];

    extension(RegexBuilder regexBuilder)
    {
        public RegexBuilder PossibleHtmlWhitespace() =>
            regexBuilder.AnyOf(HtmlWhitespaceCharacters, RegexQuantifier.ZeroOrMore);

        public RegexBuilder HtmlOpenTag(string tagName) =>
            HtmlTag(regexBuilder, tagName, true);

        public RegexBuilder HtmlCloseTag(string tagName) =>
            HtmlTag(regexBuilder, tagName, false);
    }

    private static RegexBuilder HtmlTag(RegexBuilder regexBuilder, string tagName, bool opening) =>
        regexBuilder
            .Text(opening ? "<" : "</")
            .PossibleWhitespace()
            .Text(tagName)
            .WordBoundary()
            .AnyCharacterExcept(">", RegexQuantifier.ZeroOrMore)
            .Text(">");
}
