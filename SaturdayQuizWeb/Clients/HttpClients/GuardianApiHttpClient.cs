using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

public interface IGuardianApiHttpClient : IGuardianHttpClient;

public class GuardianApiHttpClient(HttpClient httpClient, IOptions<GuardianConfig> configOptions)
    : IGuardianApiHttpClient
{
    public async Task<string> GetStringAsync(string endpoint)
    {
        httpClient.BaseAddress ??= new Uri(configOptions.Value.ApiBaseUrl);
        return await httpClient.GetStringAsync(endpoint);
    }
}