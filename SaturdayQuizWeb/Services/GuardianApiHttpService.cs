using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model.Api;

namespace SaturdayQuizWeb.Services;

public interface IGuardianApiHttpService
{
    Task<GuardianApiResponse?> ListQuizzesAsync(int pageSize = 5);
}

public class GuardianApiHttpService : IGuardianApiHttpService
{
    private readonly RestClient _restClient;
    private readonly SaturdayQuizConfig _config;

    public GuardianApiHttpService(RestClient restClient, IOptions<SaturdayQuizConfig> configOptions)
    {
        _restClient = restClient;
        _config = configOptions.Value;
    }

    public async Task<GuardianApiResponse?> ListQuizzesAsync(int pageSize = 5)
    {
        var request = new RestRequest("series/the-quiz-thomas-eaton")
            {
                RequestFormat = DataFormat.Json
            }
            .AddQueryParameter("api-key", _config.GuardianApiKey)
            .AddQueryParameter("page-size", pageSize.ToString());
        var response = await _restClient.ExecuteGetAsync<GuardianApiResponse>(request);
        return response.IsSuccessful ? response.Data : null;
    }
}
