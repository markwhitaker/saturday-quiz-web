using RestSharp;
using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

public class GuardianWebsiteHttpClient(IOptions<GuardianConfig> configOptions) : IGuardianWebsiteHttpClient
{
    private readonly RestClient _restClient = new(configOptions.Value.WebsiteBaseUrl)
    {
        AcceptedContentTypes = [MimeType.Text.Html]
    };

    public async Task<string> GetStringAsync(string endpoint)
    {
        var restRequest = new RestRequest(endpoint);
        var restResponse = await _restClient.ExecuteAsync(restRequest);
        return restResponse.Content ?? string.Empty;
    }
}
