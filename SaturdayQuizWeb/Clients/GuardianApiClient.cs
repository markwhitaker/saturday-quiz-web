using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianApiClient : IGuardianQuizMetadataClient;

/// <summary>
/// A typed HTTP client: see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0#typed-clients
/// </summary>
public class GuardianApiClient : IGuardianApiClient
{
    private readonly HttpClient _httpClient;
    private readonly GuardianConfig _config;
    private readonly ILogger<GuardianApiClient> _logger;

    public GuardianApiClient(
        HttpClient httpClient,
        IOptions<GuardianConfig> configOptions,
        ILogger<GuardianApiClient> logger)
    {
        _config = configOptions.Value;
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(_config.ApiBaseUrl);
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var url = $"{_config.ApiEndpoint}?api-key={_config.ApiKey}&page-size={count}";
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        var httpResponse = await _httpClient.SendAsync(httpRequest);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<GuardianApiResponse>(responseJson);
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
                    .Distinct()
                    .OrderByDescending(quizMetadata => quizMetadata.Date)
                    .Take(count)
                    .ToList();
            }
        }
        else
        {
            _logger.LogError("Failed to get quiz metadata from Guardian API: {StatusCode} {ReasonPhrase}",
                (int)httpResponse.StatusCode, httpResponse.ReasonPhrase);
        }

        return Array.Empty<QuizMetadata>();
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
