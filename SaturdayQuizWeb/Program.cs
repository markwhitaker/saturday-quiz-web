using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Dtos;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Models;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Utils;
using SaturdayQuizWeb.Wrappers;

var builder = WebApplication.CreateBuilder(args);
SetupServices(builder.Services, builder.Configuration);

var app = builder.Build();
SetupApp(app);

app.MapGet("/api/quiz/", async (
        HttpContext httpContext,
        IQuizService quizService) =>
    {
        if (httpContext.Request.QueryString.HasValue)
        {
            return Results.BadRequest();
        }

        httpContext.Response.AddCustomHeaders(TimeSpan.Zero);

        try
        {
            app.Logger.LogInformation("Getting latest quiz");
            var quiz = await quizService.GetLatestQuizAsync();
            var quizDto = new QuizDto(quiz);
            return Results.Ok(quizDto);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error getting latest quiz");
            return Results.NotFound();
        }
    })
    .Produces<QuizDto>(contentType: MimeType.Application.Json)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithTags("Quiz")
    .WithName("GetLatestQuiz")
    .WithDisplayName("Get latest quiz")
    .WithDescription("Get the latest quiz")
    .WithOpenApi();

app.MapGet("/api/quiz/{date}", async (
        HttpContext httpContext,
        IQuizService quizService,
        string date) =>
    {
        if (httpContext.Request.QueryString.HasValue)
        {
            return Results.BadRequest();
        }

        // Check if date is valid with format yyyy-MM-dd
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return Results.BadRequest();
        }

        httpContext.Response.AddCustomHeaders(TimeSpan.FromDays(365));

        try
        {
            app.Logger.LogInformation("Getting quiz for date {date}", date);
            var quiz = await quizService.GetQuizAsync(parsedDate);
            var quizDto = new QuizDto(quiz);
            return Results.Ok(quizDto);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error getting quiz for date {date}", date);
            return Results.NotFound();
        }
    })
    .Produces<QuizDto>(contentType: MimeType.Application.Json)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithTags("Quiz")
    .WithName("GetQuizByDate")
    .WithDisplayName("Get quiz by date")
    .WithDescription("Get quiz by date")
    .WithOpenApi(operation =>
    {
        var dateParam = operation.Parameters.First();
        dateParam.Description = "Date in yyyy-MM-dd format";
        return operation;
    });

app.MapGet("/api/quiz-metadata", async (
        HttpContext httpContext,
        IQuizMetadataService quizMetadataService,
        int count = 10) =>
    {
        httpContext.Response.AddCustomHeaders(TimeSpan.Zero);
        var quizNoun = count == 1 ? "quiz" : "quizzes";

        try
        {
            app.Logger.LogInformation("Getting quiz metadata for last {count} {quizNoun}...", count, quizNoun);
            var quizMetadata = await quizMetadataService.GetQuizMetadataAsync(count);
            var quizMetadataDtos = quizMetadata.Select(q => new QuizMetadataDto(q)).ToArray();
            return Results.Ok(quizMetadataDtos);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error getting quiz metadata for last {count} {quizNoun}", count, quizNoun);
            return Results.BadRequest();
        }
    })
    .Produces<QuizMetadata[]>(contentType: MimeType.Application.Json)
    .Produces(StatusCodes.Status400BadRequest)
    .WithTags("Quiz Metadata")
    .WithName("GetQuizMetadata")
    .WithDisplayName("Get quiz metadata")
    .WithDescription("Get quiz metadata for the most recent specified number of quizzes, or omit count to get the last 10")
    .WithOpenApi();

await app.RunAsync();

public partial class Program
{
    private static void SetupServices(IServiceCollection services, ConfigurationManager configuration)
    {
        configuration
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

        services.Configure<GuardianConfig>(configuration.GetSection(Constants.ConfigSectionGuardian));

        services.AddHttpClient<IGuardianApiHttpClient, GuardianApiHttpClient>();

        services.AddSingleton<IGuardianWebsiteHttpClient, GuardianWebsiteHttpClient>();
        services.AddSingleton<IDateTimeWrapper, DateTimeWrapper>();
        services.AddSingleton<IGuardianApiClient, GuardianApiClient>();
        services.AddSingleton<IGuardianRssClient, GuardianRssClient>();
        services.AddSingleton<IHtmlService, HtmlService>();
        services.AddSingleton<IHtmlStripper, HtmlStripper>();
        services.AddSingleton<IQuestionAssembler, QuestionAssembler>();
        services.AddSingleton<IQuizMetadataService, QuizMetadataService>();
        services.AddSingleton<IQuizService, QuizService>();
        services.AddSingleton<ISectionExtractor, SectionExtractor>();
        services.AddSingleton<ISectionSplitter, SectionSplitter>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Saturday Quiz API",
                Version = "v1"
            });
        });
    }

    private static void SetupApp(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = new Utf8ContentTypeProvider(),
            OnPrepareResponse = context => context.Context.Response.AddCustomHeaders(TimeSpan.FromDays(30))
        });
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Saturday Quiz API";
        });
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
    }
}
