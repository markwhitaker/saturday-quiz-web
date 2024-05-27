// using SaturdayQuizWeb;
//
// // See https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/top-level-statements
// // for top-level statement syntax
//
// CreateHostBuilder(args).Build().Run();
// return;
//
// IHostBuilder CreateHostBuilder(string[] args) => Host
//     // This step adds UserSecrets as a configuration source
//     // (see https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets#register-the-user-secrets-configuration-source)
//     // It also registers environment variables as a configuration source (see method comment)
//     .CreateDefaultBuilder(args)
//     .ConfigureWebHostDefaults(webBuilder =>
//     {
//         webBuilder.UseStartup<Startup>();
//     });

using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Clients.HttpClients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Utils;
using SaturdayQuizWeb.Wrappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
RegisterDependencies(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new Utf8ContentTypeProvider(),
    OnPrepareResponse = context => context.Context.Response.AddCustomHeaders(TimeSpan.FromDays(30))
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/quiz/", async (string? id, IQuizService quizService) =>
    {
        // Response.AddCustomHeaders(id == null ? TimeSpan.Zero : TimeSpan.FromDays(365));

        try
        {
            // logger.LogInformation("Getting quiz with ID={id}...", id);
            var quiz = await quizService.GetQuizAsync(id);
            return Results.Ok(quiz);
        }
        catch (Exception e)
        {
            // logger.LogError(e, "Error getting quiz with ID={id}", id);
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }

    })
    .WithName("GetQuiz")
    .WithOpenApi();

app.MapGet("/api/quiz-metadata", async ([FromQuery] int? count, IQuizMetadataService quizMetadataService) =>
    {
        var metadata = await quizMetadataService.GetQuizMetadataAsync(count ?? 10);
        return Results.Ok(metadata);
    })
    .WithName("GetQuizMetadata")
    .WithOpenApi();

await app.RunAsync();
return;

void RegisterDependencies(IServiceCollection services, IConfigurationManager configuration)
{
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
}
