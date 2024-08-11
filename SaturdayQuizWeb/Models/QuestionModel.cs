using RegexToolbox.Extensions;

namespace SaturdayQuizWeb.Models;

public sealed record QuestionModel
{
    public int Number { get; init; }

    public QuestionType Type { get; init; }

    public string Question { get; init; } = string.Empty;

    public IEnumerable<string> WhatLinks => Type == QuestionType.WhatLinks
        ? Question.Remove(QuestionMarkAtEndOfString).Split(";", StringSplitOptions.TrimEntries)
        : [];

    public string Answer { get; set; } = string.Empty;

    private static readonly Regex QuestionMarkAtEndOfString = new(@"\?$", RegexOptions.Compiled);
}
