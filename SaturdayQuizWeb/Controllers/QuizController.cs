using Microsoft.Extensions.Logging;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.Controllers;

[Route("api/quiz")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly ILogger _logger;

    public QuizController(IQuizService quizService, ILogger<QuizController> logger)
    {
        _quizService = quizService;
        _logger = logger;
    }

    // GET /api/quiz
    [HttpGet]
    public async Task<ActionResult<Quiz>> GetById([FromQuery] string? id = null)
    {
        Response.AddCustomHeaders(id == null ? TimeSpan.Zero : TimeSpan.FromDays(365));

        try
        {
            _logger.LogInformation("Getting quiz with ID={id}...", id);
            var quiz = await _quizService.GetQuizAsync(id);
            return Ok(quiz);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting quiz with ID={id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new Error(e));
        }
    }
}
