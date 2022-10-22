using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianApiClient : IGuardianQuizMetadataClient
{
}

public class GuardianApiClient : IGuardianApiClient
{
    private readonly GuardianConfig _config;
    private readonly RestClient _restClient;

    public GuardianApiClient(IOptions<GuardianConfig> configOptions)
    {
        _config = configOptions.Value;
        _restClient = new RestClient(_config.ApiBaseUrl);
    }

    public async Task<IReadOnlySet<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var request = new RestRequest(_config.ApiEndpoint)
            {
                RequestFormat = DataFormat.Json
            }
            .AddQueryParameter("api-key", _config.ApiKey)
            .AddQueryParameter("page-size", count.ToString());
        var response = await _restClient.ExecuteGetAsync<GuardianApiResponse>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data.Results
                .Select(item => new QuizMetadata
                {
                    Id = item.Id,
                    Title = item.WebTitle,
                    Date = item.WebPublicationDate,
                    Url = item.WebUrl,
                    Source = "API"
                })
                .Distinct()
                .OrderByDescending(qm => qm.Date)
                .Take(count)
                .ToHashSet();
        }

        return new HashSet<QuizMetadata>();
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public record GuardianApiResponse
    {
        public class ResponseBody
        {
            public List<GuardianApiQuizSummary> Results { get; init; } = new();
        }

        public ResponseBody Response { get; init; } = new();

        public IEnumerable<GuardianApiQuizSummary> Results => Response.Results;
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public record GuardianApiQuizSummary
    {
        public string Id { get; init; } = string.Empty;
        public DateTime WebPublicationDate { get; init; }
        public string WebTitle { get; init; } = string.Empty;
        public string WebUrl { get; init; } = string.Empty;
    }
}
