using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SaturdayQuizWeb
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            // This step adds UserSecrets as a configuration source
            // (see https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets#register-the-user-secrets-configuration-source)
            // It also registers environment variables as a configuration source (see method comment)
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
