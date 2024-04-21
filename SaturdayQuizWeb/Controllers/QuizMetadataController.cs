using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.Controllers;

[Route("api/quiz-metadata")]
[ApiController]
public class QuizMetadataController : ControllerBase
{
    private const int DefaultQuizCount = 10;

    private readonly IQuizMetadataService _quizMetadataService;
    private readonly ILogger _logger;

    public QuizMetadataController(IQuizMetadataService quizMetadataService, ILogger<QuizMetadataController> logger)
    {
        _quizMetadataService = quizMetadataService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizMetadata>>> GetQuizMetadataAsync([FromQuery] int count = DefaultQuizCount)
    {
        Response.AddCustomHeaders(TimeSpan.Zero);
        var quizNoun = count == 1 ? "quiz" : "quizzes";

        try
        {
            _logger.LogInformation("Getting quiz metadata for last {count} {quizNoun}...", count, quizNoun);
            var quizMetadata = await _quizMetadataService.GetQuizMetadataAsync(count);
            return Ok(quizMetadata);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting quiz metadata for last {count} {quizNoun}", count, quizNoun);
            return StatusCode((int)HttpStatusCode.InternalServerError, new Error(e));
        }
    }
}
