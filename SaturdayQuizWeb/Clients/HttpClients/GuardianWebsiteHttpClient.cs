using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Clients.HttpClients;

public interface IGuardianWebsiteHttpClient : IGuardianHttpClient;

public class GuardianWebsiteHttpClient : IGuardianWebsiteHttpClient
{
    private readonly HttpClient _httpClient;

    public GuardianWebsiteHttpClient(IOptions<GuardianConfig> configOptions)
    {
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(configOptions.Value.WebsiteBaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.ParseAdd(MimeType.Text.Html);
        _httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
        _httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("deflate");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("UserAgent");
    }

    public async Task<string> GetStringAsync(string endpoint) => await _httpClient.GetStringAsync(endpoint);
}