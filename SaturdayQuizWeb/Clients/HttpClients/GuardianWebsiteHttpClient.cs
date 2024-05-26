using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

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
