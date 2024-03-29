﻿using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Model;
using SaturdayQuizWeb.Services;

namespace SaturdayQuizWeb.Controllers;

[Route("api/quiz")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    // GET /api/quiz
    [HttpGet]
    public async Task<ActionResult<Quiz>> GetById(
        [FromQuery] string? id = null,
        [FromQuery] int? delaySeconds = null)
    {
        Response.AddCustomHeaders(id == null ? TimeSpan.Zero : TimeSpan.FromDays(365));

        try
        {
            if (delaySeconds != null)
            {
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds.Value));
            }
            var quiz = await _quizService.GetQuizAsync(id);
            return Ok(quiz);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e}");
            return StatusCode((int)HttpStatusCode.InternalServerError, new Error(e));
        }
    }
}
