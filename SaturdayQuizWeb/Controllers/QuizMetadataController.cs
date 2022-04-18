using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

    public QuizMetadataController(IQuizMetadataService quizMetadataService)
    {
        _quizMetadataService = quizMetadataService;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuizMetadata>>> GetQuizMetadataAsync([FromQuery] int count = DefaultQuizCount)
    {
        HttpContext.Response.AddCustomHeaders();

        try
        {
            var quizMetadata = await _quizMetadataService.GetQuizMetadataAsync(count);
            return Ok(quizMetadata);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e}");
            return StatusCode(500, new Error(e));
        }
    }
}
