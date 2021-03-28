using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestSharp;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Services.Parsing;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
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
            app.UseStaticFiles();
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddHttpClient<IGuardianScraperHttpService, GuardianScraperHttpService>();

            services.AddSingleton<IRestClient>(new RestClient("https://content.guardianapis.com/theguardian/"));

            services.AddSingleton<IConfigVariables, ConfigVariables>();
            services.AddSingleton<IGuardianApiHttpService, GuardianApiHttpService>();
            services.AddSingleton<IHtmlService, HtmlService>();
            services.AddSingleton<IHtmlStripper, HtmlStripper>();
            services.AddSingleton<IQuestionAssembler, QuestionAssembler>();
            services.AddSingleton<IQuizMetadataService, QuizMetadataService>();
            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<ISectionExtractor, SectionExtractor>();
            services.AddSingleton<ISectionSplitter, SectionSplitter>();
        }
    }
}
