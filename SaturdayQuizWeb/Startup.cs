using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RestSharp;
using SaturdayQuizWeb.Services;
using SaturdayQuizWeb.Utils;

namespace SaturdayQuizWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guardian Quiz API v1"); });
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddHttpClient<IGuardianScraperHttpService, GuardianScraperHttpService>();
            services.AddSingleton<IGuardianApiHttpService, GuardianApiHttpService>();
            services.AddSingleton<IConfigVariables, ConfigVariables>();
            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<IQuizMetadataService, QuizMetadataService>();
            services.AddSingleton<IHtmlService, HtmlService>();
            services.AddSingleton<IRestClient>(new RestClient("https://content.guardianapis.com/theguardian/"));
        }
    }
}