using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SaturdayQuizWeb.Model.Api
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    public record GuardianApiResponse
    {
        public class ResponseBody
        {
            public List<GuardianApiQuizSummary> Results { get; init; }
        }

        public ResponseBody Response { get; init; }

        public IEnumerable<GuardianApiQuizSummary> Results => Response.Results;
    }
}