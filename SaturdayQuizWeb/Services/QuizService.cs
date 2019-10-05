using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SaturdayQuizWeb.Model;

namespace SaturdayQuizWeb.Services
{
    public interface IQuizService
    {
        Task<Quiz> GetQuiz(string apiKey, string id = null);
        Task<Quiz> GetQuiz(QuizMetadata quizMetadata);
    }

    public class QuizService : IQuizService
    {
        private const string GuardianUrlBase = "https://www.theguardian.com/";
        
        private readonly HttpClient _httpClient;
        private readonly IHtmlService _htmlService;
        private readonly IQuizMetadataService _quizMetadataService;

        public QuizService(IHttpClientFactory httpClientFactory, IHtmlService htmlService, IQuizMetadataService quizMetadataService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _htmlService = htmlService;
            _quizMetadataService = quizMetadataService;
        }
        
        public async Task<Quiz> GetQuiz(string apiKey, string id = null)
        {
            QuizMetadata quizMetadata;
            
            if (id == null)
            {
                var quizMetadataList = await _quizMetadataService.GetQuizMetadata(apiKey, 1);
                quizMetadata = quizMetadataList.First();
            }
            else
            {
                quizMetadata = new QuizMetadata
                {
                    Id = id
                };
            }

            return await GetQuiz(quizMetadata);
        }

        public async Task<Quiz> GetQuiz(QuizMetadata quizMetadata)
        {
            var quizHtml = await _httpClient.GetStringAsync(GuardianUrlBase + quizMetadata.Id);
            var questions = _htmlService.FindQuestions(quizHtml);
            return new Quiz
            {
                Id = quizMetadata.Id,
                Date = quizMetadata.Date,
                Title = quizMetadata.Title,
                Questions = questions
            };
        }
    }
}