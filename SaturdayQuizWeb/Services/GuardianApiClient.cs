using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Model.Api;

namespace SaturdayQuizWeb.Services;

public interface IGuardianApiClient
{
    Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int pageSize = 5);
}

public class GuardianApiClient : IGuardianApiClient
{
    private readonly GuardianConfig _config;
    private readonly RestClient _restClient;

    public GuardianApiClient(IOptions<GuardianConfig> configOptions)
    {
        _config = configOptions.Value;
        _restClient = new RestClient(_config.ApiBaseUrl);
    }

    public async Task<IReadOnlyList<QuizMetadata>> GetQuizMetadataAsync(int pageSize = 5)
    {
        var request = new RestRequest(_config.ApiEndpoint)
            {
                RequestFormat = DataFormat.Json
            }
            .AddQueryParameter("api-key", _config.ApiKey)
            .AddQueryParameter("page-size", pageSize.ToString());
        var response = await _restClient.ExecuteGetAsync<GuardianApiResponse>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data.Results
                .Select(item => new QuizMetadata
                {
                    Id = item.Id,
                    Title = item.WebTitle,
                    Date = item.WebPublicationDate,
                    Url = item.WebUrl
                })
                .OrderByDescending(qm => qm.Date)
                .ToList();
        }

        return new List<QuizMetadata>();
    }
}
