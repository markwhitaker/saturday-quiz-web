using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace SaturdayQuizWeb.Services;

public interface IGuardianScraperHttpService
{
    Task<string> GetQuizPageContentAsync(string quizId);
}

/// <summary>
/// A typed HTTP client: see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0#typed-clients
/// </summary>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class GuardianScraperHttpService : IGuardianScraperHttpService
{
    private readonly HttpClient _httpClient;

    public GuardianScraperHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://www.theguardian.com/");
    }

    public async Task<string> GetQuizPageContentAsync(string quizId) =>
        await _httpClient.GetStringAsync(quizId);
}