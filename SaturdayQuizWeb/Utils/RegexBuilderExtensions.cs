using System.Collections.Generic;
using RegexToolbox;
using static RegexToolbox.RegexQuantifier;

namespace SaturdayQuizWeb.Utils;

public static class RegexBuilderExtensions
{
    private static readonly IEnumerable<string> HtmlWhitespaceCharacters =
        new[] {" ", @"\t", @"\n", @"\r", "&nbsp;"};

    public static RegexBuilder PossibleHtmlWhitespace(this RegexBuilder regexBuilder) =>
        regexBuilder.AnyOf(HtmlWhitespaceCharacters, ZeroOrMore);

    public static RegexBuilder HtmlOpenTag(this RegexBuilder regexBuilder, string tagName) =>
        HtmlTag(regexBuilder, tagName, true);

    public static RegexBuilder HtmlCloseTag(this RegexBuilder regexBuilder, string tagName) =>
        HtmlTag(regexBuilder, tagName, false);

    private static RegexBuilder HtmlTag(RegexBuilder regexBuilder, string tagName, bool opening) =>
        regexBuilder
            .Text(opening ? "<" : "</")
            .PossibleWhitespace()
            .Text(tagName)
            .WordBoundary()
            .AnyCharacterExcept(">", ZeroOrMore)
            .Text(">");
}