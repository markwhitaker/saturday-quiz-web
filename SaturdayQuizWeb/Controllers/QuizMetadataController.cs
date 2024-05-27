using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.Controllers;

[Route("api/quiz-metadata")]
[ApiController]
public class QuizMetadataController(IQuizMetadataService quizMetadataService, ILogger<QuizMetadataController> logger)
    : ControllerBase
{
    private const int DefaultQuizCount = 10;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuizMetadata>>> GetQuizMetadataAsync([FromQuery] int count = DefaultQuizCount)
    {
        Response.AddCustomHeaders(TimeSpan.Zero);
        var quizNoun = count == 1 ? "quiz" : "quizzes";

        try
        {
            logger.LogInformation("Getting quiz metadata for last {count} {quizNoun}...", count, quizNoun);
            var quizMetadata = await quizMetadataService.GetQuizMetadataAsync(count);
            return Ok(quizMetadata);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting quiz metadata for last {count} {quizNoun}", count, quizNoun);
            return StatusCode((int)HttpStatusCode.InternalServerError, new Error(e));
        }
    }
}
