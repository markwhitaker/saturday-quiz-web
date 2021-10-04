using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SaturdayQuizWeb.Model.Api
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    public class GuardianApiResponse
    {
        public class ResponseBody
        {
            public List<GuardianApiQuizSummary> Results { get; set; }
        }

        public ResponseBody Response { get; set; }

        public IEnumerable<GuardianApiQuizSummary> Results => Response.Results;
    }
}