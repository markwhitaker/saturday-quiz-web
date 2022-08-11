using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SaturdayQuizWeb;

// See https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/top-level-statements
// for top-level statement syntax

CreateHostBuilder(args).Build().Run();

IHostBuilder CreateHostBuilder(string[] args) => Host
    // This step adds UserSecrets as a configuration source
    // (see https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets#register-the-user-secrets-configuration-source)
    // It also registers environment variables as a configuration source (see method comment)
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
