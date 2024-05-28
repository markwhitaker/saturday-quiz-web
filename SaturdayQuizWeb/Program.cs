using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Utils;
using SaturdayQuizWeb.Wrappers;

var builder = WebApplication.CreateBuilder(args);
SetupServices(builder.Services, builder.Configuration);

var app = builder.Build();
SetupApp(app);

app.MapGet("/api/quiz/", async (
        string? id,
        HttpContext httpContext,
        IQuizService quizService) =>
    {
        httpContext.Response.AddCustomHeaders(id == null ? TimeSpan.Zero : TimeSpan.FromDays(365));

        try
        {
            app.Logger.LogInformation("Getting quiz with ID={id}...", id);
            var quiz = await quizService.GetQuizAsync(id);
            return Results.Ok(quiz);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error getting quiz with ID={id}", id);
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }

    })
    .WithTags("Quiz")
    .WithName("GetQuiz")
    .WithDisplayName("Get quiz")
    .WithDescription("Get a quiz by ID, or omit the ID to get the latest quiz")
    .WithOpenApi();

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
            return Results.Ok(quizMetadata);
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error getting quiz metadata for last {count} {quizNoun}", count, quizNoun);
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    })
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
        services.AddHttpClient<IGuardianWebsiteHttpClient, GuardianWebsiteHttpClient>();

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
