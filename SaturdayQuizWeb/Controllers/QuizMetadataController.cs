using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb.Controllers
{
    [Route("api/quiz-metadata")]
    [ApiController]
    public class QuizMetadataController : ControllerBase
    {
        private const int DefaultQuizCount = 10;
        
        private readonly IQuizMetadataService _quizMetadataService;
        private readonly IConfigVariables _configVariables;

        public QuizMetadataController(IQuizMetadataService quizMetadataService, IConfigVariables configVariables)
        {
            _quizMetadataService = quizMetadataService;
            _configVariables = configVariables;
        }

        [HttpGet]
        public async Task<ActionResult<List<QuizMetadata>>> GetQuizMetadata([FromQuery] int count = DefaultQuizCount)
        {
            return await _quizMetadataService.GetQuizMetadata(_configVariables.GuardianApiKey, count);
        }
    }
}