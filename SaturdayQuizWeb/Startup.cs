using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SaturdayQuizWeb.Clients;
using SaturdayQuizWeb.Config;
using SaturdayQuizWeb.Extensions;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Utils;
using SaturdayQuizWeb.Wrappers;

namespace SaturdayQuizWeb;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Guardian Quiz API",
                Version = "v1"
            });
        });

        RegisterDependencies(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guardian Quiz API v1"); });

        // This lets us serve up index.html from wwwroot
        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = new Utf8ContentTypeProvider(),
            OnPrepareResponse = context => context.Context.Response.AddCustomHeaders()
        });
    }

    private void RegisterDependencies(IServiceCollection services)
    {
        services.Configure<GuardianConfig>(_configuration.GetSection(Constants.ConfigSectionGuardian));
        services.AddHttpClient<IGuardianWebsiteClient, GuardianWebsiteClient>();

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
}
