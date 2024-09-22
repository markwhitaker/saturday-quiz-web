using RegexToolbox.Extensions;

namespace SaturdayQuizWeb.Models;

public sealed record QuestionModel
{
    public required int Number { get; init; }

    public required QuestionType Type { get; init; }

    public required string Question { get; init; } = string.Empty;

    public IEnumerable<string> WhatLinks => Type == QuestionType.WhatLinks
        ? Question.Remove(QuestionMarkAtEndOfString).Split(";", StringSplitOptions.TrimEntries)
        : [];

    public string Answer { get; set; } = string.Empty;

    private static readonly Regex QuestionMarkAtEndOfString = new(@"\?$", RegexOptions.Compiled);
}
