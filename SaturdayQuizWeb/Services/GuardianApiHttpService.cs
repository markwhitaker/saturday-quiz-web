using System.Threading.Tasks;
using RestSharp;
using SaturdayQuizWeb.Model.Api;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Services
{
    public interface IGuardianApiHttpService
    {
        Task<GuardianApiResponse> ListQuizzesAsync(int pageSize = 5);
    }
    
    public class GuardianApiHttpService : IGuardianApiHttpService
    {
        private readonly IRestClient _restClient;
        private readonly IConfigVariables _configVariables;

        public GuardianApiHttpService(IRestClient restClient, IConfigVariables configVariables)
        {
            _restClient = restClient;
            _configVariables = configVariables;
        }

        public async Task<GuardianApiResponse> ListQuizzesAsync(int pageSize = 5)
        {
            var request = new RestRequest("series/the-quiz-thomas-eaton", DataFormat.Json)
                .AddQueryParameter("api-key", _configVariables.GuardianApiKey)
                .AddQueryParameter("page-size", pageSize.ToString());
            var response = await _restClient.ExecuteGetTaskAsync<GuardianApiResponse>(request);
            return response.IsSuccessful ? response.Data : null;
        }
    }
}