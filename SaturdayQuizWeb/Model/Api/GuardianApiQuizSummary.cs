using System;
using System.Diagnostics.CodeAnalysis;

namespace SaturdayQuizWeb.Model.Api
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public record GuardianApiQuizSummary
    {
        public string Id { get; init; }
        public DateTime WebPublicationDate { get; init; }
        public string WebTitle { get; init; }
        public string WebUrl { get; init; }
    }
}