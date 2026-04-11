using System.Text.Json;
using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Models;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianApiClient : IGuardianQuizMetadataClient;

public class GuardianApiClient(
    IGuardianApiHttpClient httpClient,
    IOptions<GuardianConfig> configOptions,
    ILogger<GuardianApiClient> logger)
    : IGuardianApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var config = configOptions.Value;
        if (string.IsNullOrWhiteSpace(config.ApiKey))
        {
            throw new Exception("Guardian API key is not set");
        }

        var results = new List<QuizMetadata>(await FetchFromEndpointAsync(config, config.ApiEndpoint, count));

        if (!string.IsNullOrWhiteSpace(config.ApiFallbackEndpoint))
        {
            results.AddRange(await FetchFromEndpointAsync(config, config.ApiFallbackEndpoint, count));
        }

        return results
            .Distinct()
            .OrderByDescending(quizMetadata => quizMetadata.Date)
            .Take(count)
            .ToList();
    }

    private async Task<IReadOnlyList<QuizMetadata>> FetchFromEndpointAsync(GuardianConfig config, string endpoint, int count)
    {
        var url = $"{endpoint}?api-key={config.ApiKey}&page-size={count}";

        try
        {
            var responseJson = await httpClient.GetStringAsync(url);
            var response = JsonSerializer.Deserialize<GuardianApiResponse>(responseJson, JsonSerializerOptions);
            if (response is not null)
            {
                return response.Results
                    .Select(item => new QuizMetadata
                    {
                        Id = item.Id.Trim(),
                        Title = item.WebTitle.Trim(),
                        Date = item.WebPublicationDate,
                        Url = item.WebUrl.Trim(),
                        Source = Constants.SourceApi
                    })
                    .ToList();
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Failed to get quiz metadata from Guardian API endpoint {Endpoint}: {StatusCode} {StatusMessage}",
                endpoint, (int?)e.StatusCode, e.StatusCode);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get quiz metadata from Guardian API endpoint {Endpoint}", endpoint);
        }

        return [];
    }

    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public record GuardianApiResponse
    {
        public record ResponseBody
        {
            public IEnumerable<GuardianApiQuizSummary> Results { get; init; } = [];
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