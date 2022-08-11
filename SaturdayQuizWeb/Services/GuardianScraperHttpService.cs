using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SaturdayQuizWeb.Config;

namespace SaturdayQuizWeb.Services;

public interface IGuardianScraperHttpService
{
    Task<string> GetQuizPageContentAsync(string quizId);
}

/// <summary>
/// A typed HTTP client: see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0#typed-clients
/// </summary>
public class GuardianScraperHttpService : IGuardianScraperHttpService
{
    private readonly HttpClient _httpClient;

    public GuardianScraperHttpService(HttpClient httpClient, IOptions<GuardianConfig> configOptions)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configOptions.Value.WebsiteBaseUrl);
    }

    public async Task<string> GetQuizPageContentAsync(string quizId) =>
        await _httpClient.GetStringAsync(quizId);
}
