using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Clients;

public class GuardianApiClient : IGuardianApiClient
{
    private readonly IGuardianApiHttpClient _httpClient;
    private readonly IOptions<GuardianConfig> _configOptions;
    private readonly ILogger<GuardianApiClient> _logger;

    public GuardianApiClient(
        IGuardianApiHttpClient httpClient,
        IOptions<GuardianConfig> configOptions,
        ILogger<GuardianApiClient> logger)
    {
        _httpClient = httpClient;
        _configOptions = configOptions;
        _logger = logger;
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int count)
    {
        var config = _configOptions.Value;
        var url = $"{config.ApiEndpoint}?api-key={config.ApiKey}&page-size={count}";

        try
        {
            var responseJson = await _httpClient.GetStringAsync(url);
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
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Failed to get quiz metadata from Guardian API: {StatusCode} {StatusMessage}",
                (int?)e.StatusCode, e.StatusCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get quiz metadata from Guardian API");
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
