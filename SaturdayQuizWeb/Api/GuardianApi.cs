using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SaturdayQuizWeb.Api
{
    public interface IGuardianApi
    {
        Task<GuardianApiResponse> ListQuizzes(string apiKey, int pageSize = 5);
    }

    public class GuardianApi : IGuardianApi
    {
        private const string UrlBase = "https://content.guardianapis.com/theguardian/";
        private const string ResourceFormat = "series/the-quiz-thomas-eaton?api-key={0}&page-size={1}";
        
        private readonly HttpClient _httpClient;

        public GuardianApi(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(UrlBase);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        }

        public async Task<GuardianApiResponse> ListQuizzes(string apiKey, int pageSize = 5)
        {
            GuardianApiResponse apiResponse = null;
            var response = await _httpClient.GetAsync(string.Format(ResourceFormat, apiKey, pageSize));
            if (response.IsSuccessStatusCode)
            {
                apiResponse = await response.Content.ReadAsAsync<GuardianApiResponse>();
            }
            return apiResponse;
        }
    }
}
