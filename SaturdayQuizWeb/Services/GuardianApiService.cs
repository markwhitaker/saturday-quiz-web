using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model.Api;

namespace SaturdayQuizWeb.Services;

public interface IGuardianApiService
{
    Task<GuardianApiResponse?> ListQuizzesAsync(int pageSize = 5);
}

public class GuardianApiService : IGuardianApiService
{
    private readonly GuardianConfig _config;
    private readonly RestClient _restClient;

    public GuardianApiService(IOptions<GuardianConfig> configOptions)
    {
        _config = configOptions.Value;
        _restClient = new RestClient(_config.ApiBaseUrl);
    }

    public async Task<GuardianApiResponse?> ListQuizzesAsync(int pageSize = 5)
    {
        var request = new RestRequest(_config.ApiEndpoint)
            {
                RequestFormat = DataFormat.Json
            }
            .AddQueryParameter("api-key", _config.ApiKey)
            .AddQueryParameter("page-size", pageSize.ToString());
        var response = await _restClient.ExecuteGetAsync<GuardianApiResponse>(request);
        return response.IsSuccessful ? response.Data : null;
    }
}
