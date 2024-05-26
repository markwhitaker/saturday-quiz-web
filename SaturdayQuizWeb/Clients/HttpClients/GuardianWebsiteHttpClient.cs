using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

/// <summary>
/// A typed HTTP client: see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0#typed-clients
/// </summary>
public class GuardianWebsiteHttpClient : IGuardianWebsiteHttpClient
{
    private readonly HttpClient _httpClient;

    public GuardianWebsiteHttpClient(HttpClient httpClient, IOptions<GuardianConfig> configOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configOptions.Value.WebsiteBaseUrl);
    }

    public async Task<string> GetStringAsync(string endpoint) => await _httpClient.GetStringAsync(endpoint);
}
