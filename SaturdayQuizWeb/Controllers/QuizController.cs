using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.Controllers;

[Route("api/quiz")]
[ApiController]
public class QuizController(IQuizService quizService, ILogger<QuizController> logger) : ControllerBase
{
    // GET /api/quiz
    [HttpGet]
    public async Task<ActionResult<Quiz>> GetById([FromQuery] string? id = null)
    {
        Response.AddCustomHeaders(id == null ? TimeSpan.Zero : TimeSpan.FromDays(365));

        try
        {
            logger.LogInformation("Getting quiz with ID={id}...", id);
            var quiz = await quizService.GetQuizAsync(id);
            return Ok(quiz);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting quiz with ID={id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new Error(e));
        }
    }
}
