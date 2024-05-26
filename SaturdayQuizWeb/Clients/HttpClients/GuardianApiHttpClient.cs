using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

/// <summary>
/// A typed HTTP client: see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#typed-clients
/// </summary>
public class GuardianApiHttpClient : IGuardianApiHttpClient
{
    private readonly HttpClient _httpClient;

    public GuardianApiHttpClient(HttpClient httpClient, IOptions<GuardianConfig> configOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configOptions.Value.ApiBaseUrl);
    }

    public async Task<string> GetStringAsync(string endpoint) => await _httpClient.GetStringAsync(endpoint);
}
