using System;
using System.Diagnostics.CodeAnalysis;

namespace SaturdayQuizWeb.Model.Api;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public record GuardianApiQuizSummary
{
    public string Id { get; init; } = string.Empty;
    public DateTime WebPublicationDate { get; init; }
    public string WebTitle { get; init; } = string.Empty;
    public string WebUrl { get; init; } = string.Empty;
}
