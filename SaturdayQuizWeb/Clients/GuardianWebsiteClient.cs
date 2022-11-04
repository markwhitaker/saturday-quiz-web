using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients;

public interface IGuardianWebsiteClient
{
    Task<string> GetPageContentAsync(string endpoint);
}

/// <summary>
/// A typed HTTP client: see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0#typed-clients
/// </summary>
public class GuardianWebsiteClient : IGuardianWebsiteClient
{
    private readonly HttpClient _httpClient;

    public GuardianWebsiteClient(HttpClient httpClient, IOptions<GuardianConfig> configOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configOptions.Value.WebsiteBaseUrl);
    }

    public async Task<string> GetPageContentAsync(string endpoint) =>
        await _httpClient.GetStringAsync(endpoint);
}
