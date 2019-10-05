using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IConfigVariables _configVariables;

        public QuizController(IQuizService quizService, IConfigVariables configVariables)
        {
            _quizService = quizService;
            _configVariables = configVariables;
        }

        // GET /api/quiz
        [HttpGet]
        public async Task<ActionResult<Quiz>> GetById([FromQuery] string id = null)
        {
            return await _quizService.GetQuiz(_configVariables.GuardianApiKey, id);
        }
    }
}