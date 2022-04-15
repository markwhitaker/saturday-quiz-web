using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SaturdayQuizWeb.Model.Api;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public record GuardianApiResponse
{
    public class ResponseBody
    {
        public List<GuardianApiQuizSummary> Results { get; init; } = new();
    }

    public ResponseBody Response { get; init; } = new();

    public IEnumerable<GuardianApiQuizSummary> Results => Response.Results;
}
